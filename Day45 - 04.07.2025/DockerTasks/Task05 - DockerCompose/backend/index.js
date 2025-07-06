const express = require('express');
const mongoos = require('mongoose');

const app = express();
const PORT = 5000;

const MONGO_URL = process.env.MONGO_URL || 'mongodb://localhost:27017/db'


mongoos.connect(MONGO_URL)
    .then(()=>console.log("Connected to mongo"))
    .catch((err)=>console.log("Failed to connect",err));

app.get("/",(req,res)=>{
    res.json({
        message:"Hello node js is running and connected to mongodb"
    });
});

app.listen(PORT, ()=> console.log(`Server running on ${PORT}`));