'use strict';
(require('rootpath')());

var express = require('express');
var app = module.exports = express();
var http = require('http');
var config = require('server/config.js');
var request = require('request');
var fs = require('fs');
config.configure(app);

//var serverpath = 'http://localhost:11111/test';
//var serverpath = 'http://169.254.231.73:11111/test';
var serverpath = 'http://senior15.ee.cooper.edu:11111/test';
var serverpathnofeat = serverpath + "?nofeat=1";

var date = new Date();
var logfile = 'logs/log-' + date.getTime();

//initialization
app.get('/data', function(req, res, next) {
  request.get(serverpath, function(error, response, body) {
    if (!error && response.statusCode === 200) {
      res.send(body);
    } else {
      console.log(error);
      next(new Error(error));
    }
  });
  
});

app.post('/data', function(req, res, next) {
  console.log('post request with body: ');
  console.log(req.body);

  var path = serverpath + "?nresults=" + 
    req.body.nresults + "&features=" + 
    req.body.features + "&weights=" + req.body.weights;

  fs.appendFile(logfile, req.body.features + '~' + req.body.weights + '\n', function (err) {
	if (err) throw err;
  });

  request.post(path, function(error, response, body) { 
    if (!error && response.statusCode === 200) {
      res.send({movies: body}); //sends an array of json as string
    }else{
      console.log('err on post');
  	  next(new Error(error));
    }
  });
});

//static pages
require('server/static.js')(app);

var errorHandler = require('server/errorhandler.js').errorHandler;
app.use(errorHandler);
var server = http.createServer(app);

console.log('listening on port ' + config.port);
server.listen(config.port);