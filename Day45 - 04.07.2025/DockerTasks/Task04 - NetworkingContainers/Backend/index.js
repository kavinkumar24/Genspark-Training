const express = require('express');
const app =express();
const PORT = 5000;

app.get("/api/msg",(req,res)=>{
    res.json({
        message:"Hello world from backend"
    })
});

app.listen(PORT, () =>{
    console.log("Backend Running");
})