var express = require('express');
var http = require('http');
var path = require('path');
var bodyParser = require('body-parser');

var app = express();

app.use(bodyParser.urlencoded());

app.use(bodyParser.json());

app.use(express.static(__dirname));

var httpServer = http.createServer(app).listen(8080, function(req,res){
        console.log('Socket IO server has been started');
});


var io = require('socket.io').listen(httpServer);

var players = [];

var cli = {};

io.on('connection', function (socket) {
	cli[socket.id] = socket;

    var currentClient;
    socket.emit('connection', {msg: 'Connected to server' });
    console.log('new client added');

    socket.on('fromclient', function (data) {
    	socket.broadcast.emit('toclient',{msg: data.msg});
    	console.log('Message from client : '+data.msg);
    });

    socket.on('login', function(data){
        if (players.length == 0){
	        console.log('client login success : '+data.user);
	        socket.emit('loginconfirm', {user:data.user, msg:'Success', socketid:socket.id, color:data.color});
	        var player = new Object();
        	player.user = data.user;
        	player.socketid = socket.id;
        	player.color = data.color;
        	players.push(player);
	    }
	    else if (players.length == 1){
	    	if (players[0].color == data.color){
	    		socket.emit('loginconfirm', {user:data.user, msg:'Fail'});
	    	}
	    	else {
	    		console.log('client login success : '+data.user);
	        	socket.emit('loginconfirm', {user:data.user, msg:'Success', socketid:socket.id, color:data.color});
	        	var player = new Object();
        		player.user = data.user;
        		player.socketid = socket.id;
        		player.color = data.color;
        		players.push(player);
	    	}
	    }
	    else {
	    	socket.emit('loginconfirm', {user:data.user, msg:'Fail'});
	    }
    });

    socket.on('disconnect', function() {
      	console.log('Got disconnect!');
      	for(var i = 0; i < players.length; ++i) {
    		if(players[i].socketid === socket.id) {
       			players.splice(i, 1);
       			console.log("players count : "+players.length);
       		}
       	}
   	});
});

app.post('/abc', function(req, res) {
	console.log(req.body);
	for(var k in cli){
		cli[k].emit('toclient', {msg: "motherfucker"});
		console.log("fuck me " + k);
	}
	res.send({msg: "you are professional motherfucker"});
	res.end();
});
