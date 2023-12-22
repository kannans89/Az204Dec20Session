const express = require("express");
const msal = require("@azure/msal-node");

const config = {
  auth: {
    clientId: "9bdf880a-a990-4491-9713-fea1747362f7",
    authority:
      "https://login.microsoftonline.com/f0093ae3-bfa4-46e1-9b91-668278209d56",
    clientSecret: "ikX8Q~v-yXXNIk6BrmNqMotkWb7xxYTSm9ba4bZj",
  },
  system: {
    loggerOptions: {
      loggerCallback(loglevel, message, containsPii) {
        console.log(message);
      },
      piiLoggingEnabled: false,
      logLevel: msal.LogLevel.Verbose,
    },
  },
};
const pca = new msal.ConfidentialClientApplication(config);
const server = express();

server.get("/login", (req, res) => {
  // Initiate login process
  const authCodeUrlParameters = {
    scopes: ["user.read"],
    redirectUri: "http://localhost:3000/redirect",
  };

  // get url to sign user in and consent to scopes needed for applicatio
  pca
    .getAuthCodeUrl(authCodeUrlParameters)
    .then((response) => {
      res.redirect(response);
    })
    .catch((error) => console.log(JSON.stringify(error)));
});

server.get("/redirect", (req, res) => {
  // Successfully logged in
  const tokenRequest = {
    code: req.query.code,
    scopes: ["user.read"],
    redirectUri: "http://localhost:3000/redirect",
  };

  pca
    .acquireTokenByCode(tokenRequest)
    .then((response) => {
      console.log("\nResponse: \n:", response);
      res.sendStatus(200);
    })
    .catch((error) => {
      console.log(error);
      res.status(500).send(error);
    });
});

server.listen(3000);
