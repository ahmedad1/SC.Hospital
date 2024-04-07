import { DisplayAlertModal, appendLoadingIcon, backendAccountApi, getCookie, postJSON, removeLoadingIcon, setCookie } from "./shared.js";

// login
let formLog=document.querySelector('.formlog')
let userNameLog=document.querySelector('input[name=userNameLog]')
let passwordLog=document.querySelector('input[name=passwordLog]')
let loginBtn=document.querySelector(".loginBtn");
let showPassLogin=document.querySelector("#showPassLogin")
//--------------------------------------------
// sign up
let formsignup=document.querySelector('.formsignup');
let fName=document.querySelector('input[name=fname]')
let lName=document.querySelector('input[name=lname]')
let emailSign=document.querySelector('input[name=emailsign]')
let gender=document.querySelectorAll('input[name=gender]')
let passwordSign=document.querySelector('input[name=passwordsign]')
let bDate=document.querySelector('input[name=bDate]')
let userNameSign=document.querySelector("input[name=userNameSign]")
let signUpBtn=document.querySelector(".signUpBtn")
let showPassSignUp=document.querySelector("#showPassSignUp")

//-----------------------
//contactus
let contactusform=document.querySelector('.contactus')
//--------------------------
//feedback
let feedbackform=document.querySelector('.feedbackform')
//-----------------------------
let analysisanchor=document.querySelector('.analysisanchor')
 
showPassLogin.addEventListener("change",()=>{
    passwordLog.type=passwordLog.type=="password"?"text":"password"
})
showPassSignUp.addEventListener("change",()=>{
    passwordSign.type=passwordSign.type=="password"?"text":"password"
})
async function logIn (username , password){
   let result= await postJSON(`${backendAccountApi}log-in`,{"userName":username,"password":password})
    if(result.status==200)
        return await result.json();
    else return false;
    
    
}
async function signUp(firstName,lastName,email,userName,password,birthDate,gender){
    let result =await postJSON(`${backendAccountApi}sign-up`,{
    "firstName":firstName,
    "lastName":lastName,
    "email":email,
    "userName":userName,
    "password":password,
    "birthDate":birthDate,
    "gender":gender


})
if(result.status==200)
return true
else
{
    let repeated=await result.json();
    return repeated.alreadyExistField;

}
}




formLog.onsubmit=async (e)=>{
e.preventDefault();
appendLoadingIcon(loginBtn)
let result=await logIn(userNameLog.value,passwordLog.value);
removeLoadingIcon(loginBtn)
if(result==false){
DisplayAlertModal("Invalid UserName Or Password")

return;
}
else if (result.success==true &&result.emailConfirmed==false){
   
location.href=`${location.origin}/emailConfirmation.html`;
return;
}

if(getCookie("role")=="Pat")
location.href=`${location.origin}/user.html`;
else if(getCookie("role")=="Doc")
location.href=`${location.origin}/doctor.html`;
else if(getCookie("role")=="Adm")
location.href=`${location.origin}/admin.html`;


}
formsignup.onsubmit=async (e)=>{
e.preventDefault();
appendLoadingIcon(signUpBtn)
if(!(emailSign.value&&fName.value&&bDate.value&&lName.value&&userNameSign.value&&passwordSign.value)){
    removeLoadingIcon(signUpBtn)
return
}
let regexEmail=/\w+@\w+\.\w+(\.\w+)*/ig
if(!regexEmail.test(emailSign.value)){
    removeLoadingIcon(signUpBtn)
return
}
let result=await signUp(
     fName.value
    ,lName.value
    ,emailSign.value
    ,userNameSign.value
    ,passwordSign.value
    ,bDate.value
    ,gender[0].checked?gender[0].value:gender[1].value
    )
removeLoadingIcon(signUpBtn)

if(result!==true){
DisplayAlertModal(`${result} is already exist`)
return
}
setCookie("email",emailSign.value,1);
location.href=`${location.origin}/emailConfirmation.html`;

}
contactusform.onsubmit=(e)=>{
e.preventDefault();
    DisplayAlertModal('Message Sent Successfully',"text-success")
    location.reload();
}
feedbackform.onsubmit=(e)=>{
   e.preventDefault()
    DisplayAlertModal('Thank You For Your FeedBack',"text-success")
    location.reload()
}