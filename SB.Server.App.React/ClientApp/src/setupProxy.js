const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:8054';

const context =  [
    "/weatherforecast",
    "/api",
    "/token",
    "/swagger"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    target: target,
    secure: false,
    headers: {
        Connection: 'Keep-Alive',
        //https://stackoverflow.com/a/56491455
        //AccessControlExposeHeaders: '*'
    }
  });

  app.use(appProxy);
};
