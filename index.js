const express = require('express');
const path = require('path');
const app = express();

const members = require('./members')

const logger = require('./middleware/logger');

app.use(logger);
app.get('/api/members', function (req, res){
   res.json(members);
});

app.get('/api/members/:id',function (req,res) {
    res.json(members.filter(member => member.id === parseInt(req.params.id)));
});

app.use(express.static(path.join(__dirname, 'public')));

const PORT = process.env.PORT || 5000;

app.listen(PORT, ()=> console.log('Server started on port ${PORT}'));
