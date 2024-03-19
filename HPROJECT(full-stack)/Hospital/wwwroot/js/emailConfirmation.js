import { DisplayAlertModal, appendLoadingIcon, backendAccountApi, getCookie, postJSON, removeLoadingIcon } from "./shared.js"
let inpVerification=document.querySelector("#verification");
let verifyCodeBtn=document.querySelector(".verifyCodeBtn")
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
    let result=await postJSON(`${backendAccountApi}SendCode`,{"email":getCookie("email")})
    if(result.status!=200)
    DisplayAlertModal("Unable to send the message , Try Again","text-info")

}

form.onsubmit=async function(e){
    e.preventDefault();
    if(getCookie("email")==null)
    return
    appendLoadingIcon(verifyCodeBtn)
    let result=await postJSON(`${backendAccountApi}ValidateCode`,{"email":getCookie("email"),"code":inpVerification.value})
    removeLoadingIcon(verifyCodeBtn)
    if(result.status !=200){
    DisplayAlertModal("Invalid Code.... Try again")
    return;
    }
    DisplayAlertModal("Email Confirmed Successfully","text-success")
    onclick=()=>{
    location.href=`${location.origin}/index.html`}
}