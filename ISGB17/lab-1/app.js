'use strict';

const express = require('express');
const jsDOM = require('jsdom');
const fs = require('fs');
const posts = require('./blogPosts.js');
//const bodyParser = require('body-parser'); 
//Express har body-parser som dependent

let app = express();

//Middleware
app.use('/public' , express.static('public'));
app.use(express.urlencoded( {extended : true}));


app.listen(3001, function(){ // PORT 3001
    console.log('Lyssnar på port 3001... Servern är igång =)');
});

app.get('/', function(req, res){
    console.log('En utskrift från get -> index.html');
    fs.readFile(__dirname + '/index.html', function(err, data){
        if(err){
            console.log('Kunde inte hämta filen...');
        } else {
            console.log('Hämtar html-filen...');
            let htmlCode = data;
            let vDOM = new jsDOM.JSDOM(htmlCode);

            let vPath = vDOM.window.document;
            let vSection = vPath.querySelector('section');
            
            for (let i = 0; i < posts.blogPosts.length; i++){
                
                let vArticle = vPath.createElement('article');
                let vSubject = vPath.createElement('h2');
                let vAuthor = vPath.createElement('h5');
                let vDate = vPath.createElement('h6');
                let vText = vPath.createElement('p');

                //Lägger in textNoder
                let subjectTextNode = vPath.createTextNode(posts.blogPosts[i].msgSubject);
                vSubject.appendChild(subjectTextNode);
                let authorTextNode = vPath.createTextNode(posts.blogPosts[i].nickName);
                vAuthor.appendChild(authorTextNode);
                let dateTextNode = vPath.createTextNode(posts.blogPosts[i].timeStamp);
                vDate.appendChild(dateTextNode);
                let textNode = vPath.createTextNode(posts.blogPosts[i].msgBody);
                vText.appendChild(textNode);

                //Appends
                vArticle.appendChild(vSubject);
                vArticle.appendChild(vAuthor);
                vArticle.appendChild(vDate);
                vArticle.appendChild(vText);
                vSection.appendChild(vArticle);
                
            }
            //Serialize
            htmlCode = vDOM.serialize();
            res.send(htmlCode);
        }
    });
});
app.get('/skriv', function(req, res){
    console.log('get -> skriv.html')
    res.sendFile(__dirname + '/skriv.html');
});
app.post('/skriv', function(req, res){
    console.log('Post från /skriv.html...');
    console.log(req.body);
    //Validerar
    let newSubject = req.body.subject;
    let newMsgBody = req.body.msgbody;
    let newNickname = req.body.nickname;
    if(newSubject.length < 3 || newMsgBody.length < 10 || newNickname.length < 3){
        res.redirect('/skriv');
    } else {
        let date = new Date();
        console.log(date);
        let postDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
        posts.blogPosts.push({
            nickName : newNickname,
            msgSubject : newSubject,
            timeStamp : postDate,
            msgBody : newMsgBody
        });
        res.redirect('/');
    }
});
