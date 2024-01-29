'use strict';

const cookieParser = require('cookie-parser');
const express = require('express');
const fs = require('fs');
const jsdom = require('jsdom');

const app = express();
const http = require('http').createServer(app);
const io = require('socket.io')(http);
const database = require('./database.js');

// Middleware
app.use('/public', express.static('public'));
app.use('/clientscripts', express.static(__dirname));
app.use(express.urlencoded({ extended: true }));
app.use(cookieParser());

http.listen(3001, () => {
    console.log('Servern är igång...');
});

io.on('connection', (socket) => {
    console.log('Ny användare anslöt...');

    socket.on('new-user', nickName => { 
        database.users[socket.id] = nickName;
    });
    socket.on('send-chat-message', data => {
        database.messages.push({
            name: database.users[socket.id],
            message: data.message,
            postDate: data.postDate
        });
        socket.broadcast.emit('chat-message', { postDate: data.postDate, message: data.message, name: database.users[socket.id] });
    });
});

app.get('/', (req, res) => {
    const cookies = req.cookies;

    if('nick-name' in cookies) {
        fs.readFile(__dirname + '/index.html', (err, data) => {
            if(err) {
                console.log('Filen kunde inte öppnas');
            } else {
                console.log('index.html inläst');
                let htmlCode = data;
                let vDOM = new jsdom.JSDOM(htmlCode);

                for(let i = 0; i < database.messages.length; i++) {
                    let vSection = vDOM.window.document.querySelector('#message-container');
                    let vArticle = vDOM.window.document.createElement('article');
                    
                    let dateTextNode = vDOM.window.document.createTextNode(database.messages[i].postDate + " ");
                    
                    let nickElement = vDOM.window.document.createElement('strong');
                    let nickTextNode = vDOM.window.document.createTextNode(database.messages[i].name + ": ");
                    nickElement.appendChild(nickTextNode);

                    let msgTextNode = vDOM.window.document.createTextNode(database.messages[i].message);
                    
                    vArticle.appendChild(dateTextNode);
                    vArticle.appendChild(nickElement);
                    vArticle.appendChild(msgTextNode);
                    vSection.appendChild(vArticle);
                }

                htmlCode = vDOM.serialize(vDOM);
                res.send(htmlCode);
            }
        });
    } else {
        res.redirect('/login');
    }
});

app.get('/login', (req, res) => {
    res.sendFile(__dirname + '/loggain.html', (err) => {
        if(err) {
            console.log('Filen kunde inte öppnas');
        } else {
            console.log('loggain.html inläst');
        }
    });
});

app.post('/login', (req, res) => {
    let nickName = req.body.nickname;

    if(nickName.length >= 3) {
        res.cookie('nick-name', req.body.nickname, {
            maxAge: 1000 * 60 * 60
        });
        res.redirect('/');
    } else {
        res.redirect('/login');
    }
});

app.get('/favicon.ico', (req, res) => {
    res.sendFile(__dirname + '/favicon.ico', (err) => {
        if(err) {
            console.log('Filen kunde inte öppnas');
        } else {
            console.log('favicon.ico inläst');
        } 
    });
});