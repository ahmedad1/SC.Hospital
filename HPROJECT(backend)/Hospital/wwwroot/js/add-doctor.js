import { DisplayAlertModal, GetAuth, backendAccountApi, backendDepartmentApi, fetchJSONAuth, getCookie, signOut } from "./shared.js";

let signOutBtn = document.querySelector(".signout");
signOutBtn.addEventListener("click", signOut);
let usernamespan = document.querySelector(".usernamespan");
usernamespan.innerText = getCookie("firstName");
const addDoctorForm=document.getElementById("add-doctor-form")
const addDoctorBtn = document.getElementById("addDoctor");
const firstName =document.getElementById("firstName")
const lastName=document.getElementById("lastName")
const userName=document.getElementById("userName")
const email=document.getElementById("email")
const password=document.getElementById("password")
const genderSelect=document.getElementById("gender")
const departmentSelect = document.getElementById("departmentName");
const startTime=document.getElementById("startTime")
const endTime=document.getElementById("endTime")
const birthDate=document.getElementById("dateOnly")
const daysOfTheWork=document.getElementById("days")
addDoctorForm.onsubmit=e=>e.preventDefault()
addEventListener("DOMContentLoaded", async (e) => {
  e.preventDefault();
  const result = await GetAuth(`${backendDepartmentApi}no-verbos`)
  result.forEach((d) => {
    departmentSelect.insertAdjacentHTML(
      "beforeend",
      `<option value='${d.id}'selected>${d.departmentName}</option> `
    );
  });
});
function getDaysOfTheWork(daysSelect){
  let result=0;
for (let e of daysSelect.options){
  if(e.selected){
    result+= +e.value
  }
}
return result
}
addDoctorBtn.addEventListener("click", async e=>{
  e.preventDefault()
  const result=await fetchJSONAuth(`${backendAccountApi}new-doctor-account`,
  {
    "firstName": firstName.value,
    "lastName": lastName.value,
    "userName": userName.value,
    "email": email.value,
    "password": password.value,
    "gender": genderSelect.value,
    "birthDate": birthDate.value,
    "startTime": startTime.value,
    "endTime": endTime.value,
    "daysOfTheWork": getDaysOfTheWork(daysOfTheWork),
    "departmentId": departmentSelect.value
  },"POST")
  if(result.success)
    DisplayAlertModal("Added Succesfully","text-success")
  else {
    DisplayAlertModal(`The ${result.alreadyExistField} is already registered`,"text-danger")
  }
  


})