const express = require('express');
const bodyParser = require('body-parser');
const jwt = require('jsonwebtoken');

const app = express();
const port = 3000;

app.get('/', (req, res) => {
    res.send('Hello World!')
});

app.post('/token', (req, res) => {
    res.json({
        access_token: jwt.sign(username, process.env.TOKEN_SECRET, { expiresIn: '1800s' })
    });
});

app.listen(port, () => {
    console.log(`Example app listening at http://localhost:${port}`)
});