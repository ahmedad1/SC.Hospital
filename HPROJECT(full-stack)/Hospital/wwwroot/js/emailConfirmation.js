import { backendOrigin, getCookie, postJSON } from "./shared.js"
let inpVerification=document.querySelector("#verification");
let form=document.querySelector("form")
inpVerification.onkeydown=function(e){
    console.log(e)
if(!/[0-9]/ig.test( e.key)&&!((e.key.toLowerCase()=='v'||e.key.toLowerCase()=="a")&&e.ctrlKey==true)&&e.key!="Backspace"){
e.preventDefault()
}
}
inpVerification.onpaste=function(e){
    if(!/[0-9]+/ig.test(e.clipboardData.getData("text")))
    e.preventDefault();
}
onload=async function(){
    let result=await postJSON(`${backendOrigin}SendCode`,{"email":getCookie("email")})
    if(result.status!=200)
    this.alert("Unable to send the message , Try Again")

}
form.onsubmit=async function(e){
    e.preventDefault();
    if(getCookie("email")==null)
    return
    let result=await postJSON(`${backendOrigin}ValidateCode`,{"email":getCookie("email"),"code":inpVerification.value})
    
    if(result.status !=200)
    alert("Invalid Code.... Try again")
    alert("Email Confirmed Successfully")
    location.href=`${location.origin}/index.html`
}