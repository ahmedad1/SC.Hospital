import { backendOrigin, getCookie, postJSON, setCookie } from "./shared.js";

// login
let formLog=document.querySelector('.formlog')
let userNameLog=document.querySelector('input[name=userNameLog]')
let passwordLog=document.querySelector('input[name=passwordLog]')
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
//-----------------------
//contactus
let contactusform=document.querySelector('.contactus')
//--------------------------
//feedback
let feedbackform=document.querySelector('.feedbackform')
//-----------------------------
let analysisanchor=document.querySelector('.analysisanchor')
 

async function logIn (username , password){
   let result= await postJSON(`${backendOrigin}LogIn`,{"userName":username,"password":password})
    if(result.status==200)
        return await result.json();
    else return false;
    
    
}
async function signUp(firstName,lastName,email,userName,password,birthDate,gender){
    let result =await postJSON(`${backendOrigin}SignUp`,{
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
let result=await logIn(userNameLog.value,passwordLog.value);
if(result==false){
alert("Invalid UserName Or Password")
return;
}
else if (result.success==true &&result.emailConfirmed==false){
   
location.href=`${location.origin}/emailConfirmation.html`;
return;
}

if(getCookie("role")=="Pat")
location.href=`${location.origin}/user.html`;
else 
location.href=`${location.origin}/doctor.html`;

}
formsignup.onsubmit=async (e)=>{
e.preventDefault();
if(!(emailSign.value&&fName.value&&bDate.value&&lName.value&&userNameSign.value&&passwordSign.value))
return
let regexEmail=/\w+@\w+\.\w+(\.\w+)*/ig
if(!regexEmail.test(emailSign.value))
return
let result=await signUp(
     fName.value
    ,lName.value
    ,emailSign.value
    ,userNameSign.value
    ,passwordSign.value
    ,bDate.value
    ,gender[0].checked?gender[0].value:gender[1].value
    )
if(result!==true){
alert(`${result} is already exist`)
return
}
setCookie("email",emailSign.value,1);
location.href=`${location.origin}/emailConfirmation.html`;

}
contactusform.onsubmit=(e)=>{
e.preventDefault();
    alert('Message Sent Successfully')
    location.reload();
}
feedbackform.onsubmit=(e)=>{
   e.preventDefault()
    alert('Thank You For Your FeedBack')
    location.reload()
}