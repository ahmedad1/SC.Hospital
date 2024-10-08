import { checkForCookies, getCookie, signOut } from "./shared.js";

onload = checkForCookies;
let usernamespan = document.querySelector(".usernamespan");
let signout = document.querySelector(".signout");
usernamespan.innerText = getCookie("firstName");
signout.addEventListener("click", signOut);

// let send=null;
// let usernamespan=document.querySelector('.usernamespan')
// let analysiss=localStorage.analysis==undefined?[]:JSON.parse(localStorage.analysis)
// send=JSON.parse(localStorage.send)
// usernamespan.innerHTML=send.name
// let signout=document.querySelector('.signout')
// signout.onclick=()=>{
//     localStorage.send=null
//     location.href='index.html'

// }
// let tbody=document.querySelector('tbody')
// let patientdata=localStorage.doctorchoosed==undefined?[]:JSON.parse(localStorage.doctorchoosed)
// let patients=[];
// if (patientdata.length==0){
//     tbody.innerHTML=""
// }
// else{

// for(i of patientdata){
// if(i.doctoremail==send.email){
//     patients.push(i)
// }
// }
// if (patients.length==0){
//     tbody.innerHTML=""
// }
// else{
//     let tables=document.querySelectorAll('table');
//     for (K=0;K<patients.length;K++)
//     for(j =0;j< tables.length;j++){
//         if (tables[j].className.includes(patients[K].daybooked)){
//             let numofp=tables[j].children[1].children.length

// let tr=`<tr><th scope="row">${numofp+1}</th>
// <td>${patients[K].bookername}</td>
// <td>${patients[K].booker}</td>
// <td>${patients[K].daybooked}</td>
// <td>${patients[K].timebooked}</td>
// <td>${patients[K].bookerbday}</td>
// <td> <a href="#" class="btn btn-outline-success ${String.fromCharCode(97+K)}">Done</a></td>
// </tr>
// `
// // tbody.innerHTML+=tr
// table[j].children[1].innerHTML+=tr;
// for (W =0;W<tables.length ;W++){
// if (tables[W].parentElement.className.includes("active")){
//     tables[W].parentElement.classList.remove("active")
//     break
// }
// }
// table[j].parentElement.classList.add("active")
//         }
//     }

// }
// }
// let btndone=document.querySelectorAll('a.btn.btn-outline-success')

// if (btndone!=null){
// for (I of btndone)
// I.onclick=(e)=>{
// let em=e.target.parentElement.parentElement.children[2].innerText
// for (Y =0;Y<patientdata.length;Y++){
//     if (em ==patientdata[Y].booker && patientdata[Y].doctoremail==send.email){
// let firstpart=patientdata.slice(0,Y)
// let secondpart=patientdata.slice(Y+1)
// patientdata=firstpart.concat(secondpart)
// localStorage.doctorchoosed=JSON.stringify(patientdata)

// break;

// }
// }
// if (analysiss.length!=0){
//     for (j=0;j<analysiss.length;j++){
//      if (analysiss[j].doctoremail==send.email&&analysiss[j].patientemail==em){
//         let firstpart=analysiss.slice(0,j)
//         let secondpart=analysiss.slice(j+1)
//         analysiss=firstpart.concat(secondpart);
//         localStorage.analysis=JSON.stringify(analysiss)
//         break;
//      }

//     }

// }

// location.reload()

// }
// }

// let analysisinp=document.querySelector('input[type=text]')
// let emailofpatient=document.querySelector('input[type=email]')

// let formanalysis=document.querySelector('form')
// formanalysis.onsubmit=(e)=>{
// e.preventDefault();
// let r=[]
// for (H of patientdata){
// if (H.doctoremail==send.email){
//     r.push(H)
// }

// }
// if(r.length==0)
// return
// else{
//     let boole=1;
//   for(H of r){
//     if (emailofpatient.value.trim()==H.booker){
// boole=0
// break
//     }
//   }
//   if(boole){
//     alert("No one has booked you with this email")
//     return
//   }
// }

// let departmentofdoctor;
// for (B of patientdata){
// if (B.doctoremail==send.email){
// departmentofdoctor=B.department
// break
// }

// }
// let required={department:departmentofdoctor,doctorname:send.name,doctoremail:send.email,patientemail:emailofpatient.value.trim(),requiredanalysis:[]}
// if (analysiss.length==0){
//     required.requiredanalysis.push(analysisinp.value)
//     analysiss.push(required)
// localStorage.analysis=JSON.stringify(analysiss)

// }
// else{
// let a=0
// for(i =0;i< analysiss.length;i++){
//     if (analysiss[i].doctoremail==send.email&&analysiss[i].patientemail==emailofpatient.value.trim()){
//         a=1
//         analysiss[i].requiredanalysis.push(analysisinp.value)
// localStorage.analysis=JSON.stringify(analysiss)

// break;

// }
//     }
//     if (!a){
//         a=0
//         required.requiredanalysis.push(analysisinp.value)
//         analysiss.push(required)
//     localStorage.analysis=JSON.stringify(analysiss)

//     }

// }

// analysisinp.value=""
// emailofpatient.value=""

// }
