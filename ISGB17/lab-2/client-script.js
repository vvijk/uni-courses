'use strict';

const socket = io();
const messageContainer = document.getElementById('message-container');
const messageInput = document.getElementById('message-input');
const sendButton = document.getElementById('send-button');

window.addEventListener('load', () => {
    const nickName = getCookie('nick-name');
    socket.emit('new-user', nickName);

    socket.on('chat-message', data => {
        appendMessage({ postDate: data.postDate, nickName: data.name, message: data.message });
    });

    sendButton.addEventListener('click', e => {
        e.preventDefault();
        if (messageInput.value.length >= 2) {
            const message = messageInput.value;
            let date = new Date();
            let month;
            let hour;
            let minute;
            if ((date.getMonth() + 1) < 10) {
                month = '0' + (date.getMonth() + 1);
            } else {
                month = date.getMonth() + 1;
            }
            if (date.getHours() < 10) {
                hour = '0' + date.getHours();
            } else {
                hour = date.getHours();
            }
            if (date.getMinutes() < 10) {
                minute = '0' + date.getMinutes();
            } else {
                minute = date.getMinutes();
            }
            let postDate = '[' + date.getFullYear() + '-' + month + '-' + date.getDate() + ' ' + hour + ':' + minute + ']';

            appendMessage({ postDate: postDate, nickName: nickName, message: message });
            socket.emit('send-chat-message', { message: message, postDate: postDate });
            messageInput.value = '';
        } else {
            alert('Minst två tecken');
        }
    });
});

// Funktion lånad ifrån https://www.w3schools.blog/get-cookie-by-name-javascript-js
function getCookie(cookieName) {
    let cookie = {};
    document.cookie.split(';').forEach(function (cookies) {
        let [key, value] = cookies.split('=');
        cookie[key.trim()] = value;
    });
    return cookie[cookieName];
}

function appendMessage(data) {
    let dateTextNode = document.createTextNode(data.postDate + " ");

    let nickElement = document.createElement('strong');
    let nick = document.createTextNode(data.nickName + ": ");
    nickElement.appendChild(nick);

    let msgTextNode = document.createTextNode(data.message);

    let article = document.createElement('article');
    article.appendChild

    article.appendChild(dateTextNode);
    article.appendChild(nickElement);
    article.appendChild(msgTextNode);
    messageContainer.appendChild(article);
}

