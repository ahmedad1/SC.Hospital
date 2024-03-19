import { DisplayAlertModal, UpdateTokens, appendLoadingIcon, backendAccountApi, checkForCookies, getCookie, getRequiredDateFormat, patchJSON, postJSON, removeLoadingIcon, setCookie, signOut } from "./shared.js";

onload=checkForCookies
let usernamespan=document.querySelector('.usernamespan')
usernamespan.innerText=getCookie("firstName")
let signOutBtn=document.querySelector(".signout")
signOutBtn.addEventListener("click",signOut)

let firstNameInForm=document.querySelector("#firstName")
let lastNameInForm=document.querySelector ("#lastName")
let emailInForm=document.querySelector("#email")
let userNameInForm=document.querySelector("#userName")
let birthDateInForm=document.querySelector("#birthDate")
let maleOptionInForm=document.querySelector("#male")
let femaleOptionInForm=document.querySelector("#female")
let showPassword=document.querySelector("#showPassword")
let passwordVerifyingInput=document.querySelector("#verifyPassword")
let passwordChangingForm=document.querySelector(".passwordChangingForm")
let passwordVerifingForm=document.querySelector("#passwordForm")
let oldPassword=document.querySelector("#oldPassword")
let newPassword=document.querySelector("#newPassword")
let showNewPass=document.querySelector("#showNewPass")
let saveNewPasswordBtn=document.querySelector(".saveNewPassword")
showNewPass.addEventListener("change",_=>{
newPassword.type=newPassword.type=="text"?"password":"text"
})
firstNameInForm.value=getCookie("firstName");
lastNameInForm.value=getCookie("lastName");
emailInForm.value=getCookie("email");
userNameInForm.value=getCookie("userName");
birthDateInForm.value=getRequiredDateFormat(getCookie('birthDate'))
maleOptionInForm.checked=getCookie('gender')=='Male'
femaleOptionInForm.checked=getCookie('gender')=='Female'

showPassword.addEventListener('click',function(){
    passwordVerifyingInput.type=passwordVerifyingInput.type=="text"?"password":"text"
})
let submitVerifying =document.querySelector(".submitVerifying");
let submitData=document.querySelector(".submitData")
let changed={}
submitData.addEventListener('click',(e)=>{
   e.preventDefault
    if(getCookie('firstName')!=firstNameInForm.value)
    changed['firstName']=firstNameInForm.value
    if(getCookie('lastName')!=lastNameInForm.value)
    changed['lastName']=lastNameInForm.value
    if( getCookie('userName')!=userNameInForm.value)
    changed['userName']=userNameInForm.value
    if(getRequiredDateFormat(getCookie('birthDate'))!=birthDateInForm.value)
    changed['birthDate']=birthDateInForm.value
    if(maleOptionInForm.checked!=(getCookie('gender')=='Male'))
    changed['gender']=maleOptionInForm.checked?'Male':"Female"

    if(Object.keys(changed).length){
        let modal=new bootstrap.Modal(document.querySelector('.modal'))
        modal.show()
    }
    
  
})
 submitVerifying.addEventListener('click',verifyingPassword)
passwordVerifingForm.addEventListener("submit",verifyingPassword)
async function verifyingPassword(e){
    
        e.preventDefault()
       if(Object.keys(changed).length==0)
       return;
        let listOfModels=[]
    
        for(let i in changed ){
            let model={path:i,op:'replace',value:changed[i]}
            listOfModels.push(model)
    
        }
     
        if(passwordVerifyingInput.length==0){
        DisplayAlertModal("You didnt enter your password")
        return
        }
        appendLoadingIcon(submitVerifying)
        let resultOfVerifying=await postJSON(`${backendAccountApi}VerifyPassword`,passwordVerifyingInput.value)
        
        if(resultOfVerifying.status==404){
        removeLoadingIcon(submitVerifying)
        DisplayAlertModal('Wrong Password')
        return;
        }
        else if(resultOfVerifying.status==401){
            await UpdateTokens()
            resultOfVerifying=await postJSON(`${backendAccountApi}VerifyPassword`,passwordVerifyingInput.value)
            if(resultOfVerifying.status==404){
                removeLoadingIcon(submitVerifying)
                DisplayAlertModal('Wrong Password')
                return;
                }
        }
        
    
        let result=await patchJSON(`${backendAccountApi}ModifyInSensitiveData`,listOfModels)
        if(result.status!=401)
        removeLoadingIcon(submitVerifying)
        if(result.status==404){
         
            return ;
        
           
        }    
        else if (result.status==400){
        DisplayAlertModal('Username is already exist')
        return
        }
        else if(result.status==401){
            await UpdateTokens();
            result=await patchJSON(`${backendAccountApi}ModifyInSensitiveData`,listOfModels)
            removeLoadingIcon(submitVerifying)
            if(result.status==404){
                return ;
            }    
            else if (result.status==400){
            DisplayAlertModal('Username is already exist')
            return
            }
        }
        
        for(let i in changed){
            setCookie(i,changed[i],1)
        }
        usernamespan.innerText=getCookie('firstName')
        DisplayAlertModal("Changed Successfuly","text-success")

}
passwordChangingForm.addEventListener("submit",async e=>{
    e.preventDefault();
    if(newPassword.value.length<8){
        DisplayAlertModal("New Password should be at least of length 8 charaters ")
    return
    }
    appendLoadingIcon(saveNewPasswordBtn)
    let result=await postJSON(`${backendAccountApi}ChangePassword`,{"oldPassword":oldPassword.value,"newPassword":newPassword.value});
    if(result.status!=401)
    removeLoadingIcon(saveNewPasswordBtn)
    if(result.status==200)
    {
        DisplayAlertModal("Password Changed Succesfully","text-success")
    }
    else if(result.status==400){
        DisplayAlertModal("Wrong Old Password")
    }
    else if(result.status==401){
        await UpdateTokens()
        result=await postJSON(`${backendAccountApi}ChangePassword`,{"oldPassword":oldPassword.value,"newPassword":newPassword.value});
        removeLoadingIcon(saveNewPasswordBtn)
        if(result.status==200)
        {
            DisplayAlertModal("Password Changed Succesfully","text-success")
        }
        else if(result.status==400){
            DisplayAlertModal("Wrong Old Password")
        }
    }
})
