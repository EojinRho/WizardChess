var express = require('express');
var http = require('http');
var path = require('path');

var app = express();
app.use(express.static(__dirname));

var httpServer = http.createServer(app).listen(8080, function(req,res){
        console.log('Socket IO server has been started');
});


var io = require('socket.io').listen(httpServer);

io.on('connection', function (socket) {
    var currentClient;
    socket.emit('connection', {msg: 'Connected to server' });
    console.log('new client added');

    socket.on('fromclient', function (data) {
    		socket.broadcast.emit('toclient',{msg: data.msg});
    		console.log('Message from client : '+data.msg);
    });
});