

import { checkForCookies, signOut } from "./shared.js";
let signOutBtn = document.querySelector(".signout");
signOutBtn?.addEventListener("click", signOut);
onload=async()=>{
    let result=await checkForCookies(false)
    if(result)
    document.querySelector("a[href='index.html#sign']").parentElement.remove()
}
// window.addEventListener("DOMContentLoaded", async (_) => {
//   let result = await checkForCookies(false);
//   if (result) {
//     let nav = document.querySelector("nav");
//     nav.innerHTML = getNavAfterLogin(getCookie("firstName"));
//     let signOutBtn = document.querySelector(".signout");
//     signOutBtn.addEventListener("click", signOut);
//   }
// });

// let send=JSON.parse(localStorage.send)
// let nav=document.querySelector('nav')
// let section=document.querySelector('section')
// let bookbtns=document.querySelectorAll('.book')
// let dentaldata=[],ophthalmologydata=[],internaldata=[],orthopedicdata=[],neurologydata=[]
// let doctordata={};
// let doctorchoosed=localStorage.doctorchoosed==undefined?[]:JSON.parse(localStorage.doctorchoosed)
// function storedatainvar(){
//   doctordata=JSON.parse(localStorage.doctor)
//   dentaldata=doctordata.dental
//   ophthalmologydata=doctordata.ophthalmology
//   internaldata=doctordata.internal
//   orthopedicdata=doctordata.orthopedic
//   neurologydata=doctordata.neurology

// }

// onload=()=>{
//   storedatainvar();
//   if (location.hash=='#appo')
//   appoints.click();
// }

// function incrementtime(str1,str2)
// {
// let reg=/\d+/ig;
// let matched=str1.match(reg)
// let matched2=str2.match(reg);
// let m1=str1.slice(str1.length-2,str1.length)
// let m2=str2.slice(str2.length-2,str2.length)
// if (+matched[0] > +matched2[0]){

// if (m1.toUpperCase()=="Am".toUpperCase()&&m2.toUpperCase()!=m1.toUpperCase()){
//   matched2[0]=`${+matched2[0]+12}`
// }

// }
//   if(matched[0]==matched2[0]){
//     if (m1!=m2){
//       if (+matched[1]+15>=60){

//         matched[0]=`${+matched[0]+1}`
//         if (+matched[0]==13)
//           matched[0]=`1`
//         matched[1]=`${+matched[1]+15-60}`
//         if (+matched[0]==12&&+matched[1]<15)
//         m1=m1.toUpperCase()=='Am'.toUpperCase()?'Pm':'Am'
//         return matched[0]+':'+matched[1]+m1;

//       }
//       else{
//         matched[1]=`${+matched[1]+15}`
//         return matched[0]+':'+matched[1]+m1;
//       }

//     }
//     if (+matched[1]+15>= +matched2[1])
//     return -1;
//     else{
//       if (+matched[1]+15>=60){

//         matched[0]=`${+matched[0]+1}`
//         if (+matched[0]==13)
//           matched[0]=`1`
//         matched[1]=`${+matched[1]+15-60}`
//         if (+matched[0]==12&&+matched[1]<15)
//         m1=m1.toUpperCase()=='Am'.toUpperCase()?'Pm':'Am'
//         return matched[0]+':'+matched[1]+m1;

//       }
//       else{
//         matched[1]=`${+matched[1]+15}`
//         return matched[0]+':'+matched[1]+m1;
//       }
//     }
//   }
//   else if (+matched[0]<+matched2[0]){

// if (+matched[0]== +matched2[0]-1){
//   let x=null,y=null;
//   if (+matched[1]+15>=60){
//     x=+matched[0]+1
//     y=+matched[1]+15-60;

//   }
//   if (/*+matched[1]+15>+matched2[1]&&*/x!=null){
//     if (x>matched2[0]||x==matched2[0]&&y>=matched2[1]){

//       return -1;
//     }
//     else{
//       matched[0]=`${+matched[0]+1}`
//       if (+matched[0]==13)
//       matched[0]=`1`
//       matched[1]=`${+matched[1]+15-60}`
//       if (+matched[0]==12&&+matched[1]<15){

//         m1=m1.toUpperCase()=='Am'.toUpperCase()?'Pm':'Am'
//       }
//       return matched[0]+':'+matched[1]+m1;
//     }

//   }
//   else{
//     matched[1]=`${+matched[1]+15}`
//     return matched[0]+':'+matched[1]+m1;
//   }
// }
// else{
//   if (+matched[1]+15>=60){
//     matched[0]=`${+matched[0]+1}`
//     if (+matched[0]==13)
//     matched[0]=`1`
//     matched[1]=`${+matched[1]+15-60}`
//     if (+matched[0]==12&&+matched[1]<15)
//     m1=m1.toUpperCase()=='Am'.toUpperCase()?'Pm':'Am'
//     return matched[0]+':'+matched[1]+m1;
//   }
//   else{
//   matched[1]=`${+matched[1]+15}`
//   return matched[0] + ':'+ matched[1] + m1;}
// }

//   }

// else return -1
// }

// if (send !=null){

//     nav.innerHTML=`
//     `
//     section.innerHTML=`  <!-- Footer -->
//     <footer class="text-center text-white" style="background-color: #0a4275;">
//       <!-- Grid container -->
//       <div class="container p-4 pb-0">
//         <!-- Section: CTA -->
//         <section class="">
//           <p class="d-flex justify-content-center align-items-center">

//           </p>
//         </section>
//         <!-- Section: CTA -->
//       </div>
//       <!-- Grid container -->

//       <!-- Copyright -->
//       <div class="text-center p-3" style="background-color: rgba(0, 0, 0, 0.2);">
//         © 2022 Copyright:
//         <a class="text-white" href="index.html">SCHospital.com</a>
//       </div>
//       <!-- Copyright -->
//     </footer>
//     <!-- Footer -->`
// let breadcrumbbar=document.querySelectorAll('nav')
// breadcrumbbar[1].innerHTML=`<ol class="breadcrumb">
// <li class="breadcrumb-item " ><a href="user.html">Home</a></li>
// <li class="breadcrumb-item active" >Department of Orthopedic</li>

// </ol>`
// let signout=document.querySelector('.signout')
// signout.onclick=()=>{
//     localStorage.send=null
//     location.href='index.html'

// }

// }

// let appoints=document.querySelector('.appoints')

// if(appoints!=null){
// appoints.onclick=()=>{
//   // if (doctorchoosed.length==0)
//   // alert('0 Doctor Booked')
// // else{
//   let daytime=[];
//   for(L of doctorchoosed){
// if (L.booker==send.email){
//   daytime.push(L)
// }
//   }
//   // if(daytime.length==0)alert('0 Doctor Booked')
//   // else{
//     let messages="";
//     let X="";
//     document.body.innerHTML=`<nav class="navbar navbar-expand-lg bg-light navbar-light sticky-top ">
//     <a href="user.html" class="navbar-brand text-primary">S.C Hospital</a><!--special care hospital-->
//     <!-- <input type="text"placeholder=search class=" d-lg-none mr-4 "style=width:50%;outline:none;> -->
//     <a href="#N"data-toggle=collapse class="navbar-toggler"><span class="navbar-toggler-icon"></span></a>

//     <div class="collapse navbar-collapse" id="N">
//         <ul class="navbar-nav ml-auto ">
//             <li class="nav-item"><a href="user.html" class="nav-link">Home</a></li>
//             <!-- <li class="nav-item"><a href="#" class="nav-link btn btn-outline-primary text-primary rounded">Login</a></li>-->

//         </ul>
//         <!-- <a href="#login" class="nav-link btn btn-outline-primary ml-lg-2 mt-sm-3 mt-lg-0  "style=max-width:78px>Login</a> -->
//        <div class="nav-link d-flex p-0 mt-lg-0 mt-3"style="gap:5px">
//         <img src="./images/user-solid.svg"class="ml-lg-2  mt-lg-0  "width=15 alt="">
//         <span class="text-muted usernamespan"style=>${send.name}</span>

//     </div>
//     <a href="#" class=" btn btn-outline-primary ml-3 signout appsign">Sign out</a>
//     </div>
//     </nav>
//     <table class="table sat" id="table">

//     <thead>
//       <tr>
//             <th scope="col">Doctor</th>
//             <th scope="col">Day</th>
//             <th scope="col">Time</th>
//             <th scope="col">Department</th>

//           </tr>
//         </thead>
//         <tbody>

//         </tbody>
//       </table>
//     `
//     let appsign=document.querySelector('.appsign');
//     appsign.onclick=()=>{
//      localStorage.send=null
//      location.href='index.html'
//     }
//     for(Q=0;Q<daytime.length;Q++){

// X=daytime[Q].doctorname
// messages=`
// <tr id="a">
// <td> ${X}</td>
// <td>${daytime[Q].daybooked}</td>
//  <td>${daytime[Q].timebooked}</td>
//  <td>${daytime[Q].department}</td>
// </tr>`

// document.querySelector('tbody').innerHTML+=messages

// }

//   // }
// // }

// }
// }
// for (i of bookbtns){

//   i.onclick=(e)=>{

//     if (send==null){
//       alert("You must log in First")
//     return
//     }

//     e.preventDefault()
// let nameofdoc= e.target.parentElement.parentElement.children[0].children[0].innerText
// let day= e.target.parentElement.parentElement.children[1].children[6].children[0].children[1].children
// if (location.pathname.includes("dentistdep")){
// for(j of dentaldata){
//   if ( nameofdoc.toUpperCase().includes(j.name.toUpperCase())){
//     for(k of day){
//       if (k.selected==1){
//         if(k.value!="null"){
//           let pu=0;
//           for (m of doctorchoosed){
//             if (m.doctoremail==j.email&&m.booker==send.email){pu=1;break;}
//           }
// //!doctorchoosed.includes({doctoremail:j.email,daybooked:k.value ,department:"dental",booker:send.email,bookerbday:send.birthday})
//   if(!pu){
//     if (doctorchoosed.length==0){
// let avail=e.target.parentElement.parentElement.children[1].children[2]
// let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
// let time=fromto[0]+':'+fromto[1]+fromto[2]
//       doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"dental",booker:send.email,bookerbday:send.birthday,timebooked:time})
//     }
//     else{
// let objs=[];
// for (M of doctorchoosed){
// if(M.department=="dental"){
// objs.push(M)
// }
// }
// if (objs.length==0){
//   let avail=e.target.parentElement.parentElement.children[1].children[2]
// let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
// let time=fromto[0]+':'+fromto[1]+fromto[2]
//       doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"dental",booker:send.email,bookerbday:send.birthday,timebooked:time})
// }
// else{

//   // let lastele=objs[objs.length-1].timebooked
//   //********************************************** */
//   let lastele=[];
//   for (P of objs){
//     if (P.daybooked==k.value){
// lastele.push(P)
//     }
//   }
//   if (lastele.length!=0){
//     lastele=lastele[lastele.length-1].timebooked

//     let avail=e.target.parentElement.parentElement.children[1].children[2]
//     let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//     let time=fromto[3]+':'+fromto[4]+fromto[5]
//   let inc=incrementtime(lastele,time)
//   if (inc==-1){
//     alert("This day has filled with patients")
//     return;
//   }
//   else{
//     doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"dental",booker:send.email,bookerbday:send.birthday,timebooked:inc})

//   }
//   }
//   else{
//     let avail=e.target.parentElement.parentElement.children[1].children[2]
//     let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//     let time=fromto[0]+':'+fromto[1]+fromto[2]
//     doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"dental",booker:send.email,bookerbday:send.birthday,timebooked:time})

//   }

// }
// {

// }
//     }
//      localStorage.doctorchoosed=JSON.stringify(doctorchoosed)
//      location.reload()
//     }
//      else{
//       alert("You have already booked this doctor")
//      }
// }
// else alert ("You must choose a day")

//     }
//   }
//   }
// }

// }
// else if (location.pathname.includes("eyedep")){

//   for(j of ophthalmologydata){
//     if ( nameofdoc.toUpperCase().includes(j.name.toUpperCase())){

//     for(k of day){
//       if (k.selected==1){
//   if(k.value!="null"){
//     let pu=0;
//     for (m of doctorchoosed){
//       if (m.doctoremail==j.email&&m.booker==send.email){pu=1;break;}
//     }
//     if(!pu){
//       if (doctorchoosed.length==0){
//         let avail=e.target.parentElement.parentElement.children[1].children[2]
//         let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//         let time=fromto[0]+':'+fromto[1]+fromto[2]
//               doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"ophthalmology",booker:send.email,bookerbday:send.birthday,timebooked:time})
//             }
//             else{
//         let objs=[];
//         for (M of doctorchoosed){
//         if(M.department=="ophthalmology"){
//         objs.push(M)
//         }
//         }
//         if (objs.length==0){
//           let avail=e.target.parentElement.parentElement.children[1].children[2]
//         let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//         let time=fromto[0]+':'+fromto[1]+fromto[2]
//               doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"ophthalmology",booker:send.email,bookerbday:send.birthday,timebooked:time})
//         }
//         else{

//           // let lastele=objs[objs.length-1].timebooked
//           //********************************************** */
//           let lastele=[];
//           for (P of objs){
//             if (P.daybooked==k.value){
//         lastele.push(P)
//             }
//           }
//           if (lastele.length!=0){
//             lastele=lastele[lastele.length-1].timebooked

//             let avail=e.target.parentElement.parentElement.children[1].children[2]
//             let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//             let time=fromto[3]+':'+fromto[4]+fromto[5]
//           let inc=incrementtime(lastele,time)
//           if (inc==-1){
//             alert("This day has filled with patients")
//             return;
//           }
//           else{
//             doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"ophthalmology",booker:send.email,bookerbday:send.birthday,timebooked:inc})

//           }
//           }
//           else{
//             let avail=e.target.parentElement.parentElement.children[1].children[2]
//             let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//             let time=fromto[0]+':'+fromto[1]+fromto[2]
//             doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"ophthalmology",booker:send.email,bookerbday:send.birthday,timebooked:time})

//           }

//         }
//         {

//         }
//             }
//        localStorage.doctorchoosed=JSON.stringify(doctorchoosed)
//        location.reload()
//       }
//        else alert("You have already booked this doctor")
//   }
//   else alert ("You must choose a day")

//       }
//     }
//     }
//   }

// }

// else if (location.pathname.includes("internal")){

//   for(j of internaldata){
//     if ( nameofdoc.toUpperCase().includes(j.name.toUpperCase())){
//     for(k of day){
//       if (k.selected==1){
//   if(k.value!="null"){
//     let pu=0;
//     for (m of doctorchoosed){
//       if (m.doctoremail==j.email&&m.booker==send.email){pu=1;break;}
//     }
//     if(!pu){
//       if (doctorchoosed.length==0){
//         let avail=e.target.parentElement.parentElement.children[1].children[2]
//         let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//         let time=fromto[0]+':'+fromto[1]+fromto[2]
//               doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"internal",booker:send.email,bookerbday:send.birthday,timebooked:time})
//             }
//             else{
//         let objs=[];
//         for (M of doctorchoosed){
//         if(M.department=="internal"){
//         objs.push(M)
//         }
//         }
//         if (objs.length==0){
//           let avail=e.target.parentElement.parentElement.children[1].children[2]
//         let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//         let time=fromto[0]+':'+fromto[1]+fromto[2]
//               doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"internal",booker:send.email,bookerbday:send.birthday,timebooked:time})
//         }
//         else{

//           // let lastele=objs[objs.length-1].timebooked
//           //********************************************** */
//           let lastele=[];
//           for (P of objs){
//             if (P.daybooked==k.value){
//         lastele.push(P)
//             }
//           }
//           if (lastele.length!=0){
//             lastele=lastele[lastele.length-1].timebooked

//             let avail=e.target.parentElement.parentElement.children[1].children[2]
//             let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//             let time=fromto[3]+':'+fromto[4]+fromto[5]
//           let inc=incrementtime(lastele,time)
//           if (inc==-1){
//             alert("This day has filled with patients")
//             return;
//           }
//           else{
//             doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"internal",booker:send.email,bookerbday:send.birthday,timebooked:inc})

//           }
//           }
//           else{
//             let avail=e.target.parentElement.parentElement.children[1].children[2]
//             let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//             let time=fromto[0]+':'+fromto[1]+fromto[2]
//             doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"internal",booker:send.email,bookerbday:send.birthday,timebooked:time})

//           }

//         }
//         {

//         }
//             }
//        localStorage.doctorchoosed=JSON.stringify(doctorchoosed)
//        location.reload()
//     }
//     else  alert("You have already booked this doctor")
//   }
//   else alert ("You must choose a day")

//       }
//     }
//     }
//   }

// }
// else if (location.pathname.includes("bones")){

//   for(j of orthopedicdata){
//     if ( nameofdoc.toUpperCase().includes(j.name.toUpperCase())){
//     for(k of day){
//       if (k.selected==1){
//   if(k.value!="null"){
//     let pu=0;
//     for (m of doctorchoosed){
//       if (m.doctoremail==j.email&&m.booker==send.email){pu=1;break;}
//     }
//     if (!pu){
//       if (doctorchoosed.length==0){
//         let avail=e.target.parentElement.parentElement.children[1].children[2]
//         let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//         let time=fromto[0]+':'+fromto[1]+fromto[2]
//               doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"orthopedic",booker:send.email,bookerbday:send.birthday,timebooked:time})
//             }
//             else{
//         let objs=[];
//         for (M of doctorchoosed){
//         if(M.department=="orthopedic"){
//         objs.push(M)
//         }
//         }
//         if (objs.length==0){
//           let avail=e.target.parentElement.parentElement.children[1].children[2]
//         let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//         let time=fromto[0]+':'+fromto[1]+fromto[2]
//               doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"orthopedic",booker:send.email,bookerbday:send.birthday,timebooked:time})
//         }
//         else{

//           // let lastele=objs[objs.length-1].timebooked
//           //********************************************** */
//           let lastele=[];
//           for (P of objs){
//             if (P.daybooked==k.value){
//         lastele.push(P)
//             }
//           }
//           if (lastele.length!=0){
//             lastele=lastele[lastele.length-1].timebooked

//             let avail=e.target.parentElement.parentElement.children[1].children[2]
//             let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//             let time=fromto[3]+':'+fromto[4]+fromto[5]
//           let inc=incrementtime(lastele,time)
//           if (inc==-1){
//             alert("This day has filled with patients")
//             return;
//           }
//           else{
//             doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"orthopedic",booker:send.email,bookerbday:send.birthday,timebooked:inc})

//           }
//           }
//           else{
//             let avail=e.target.parentElement.parentElement.children[1].children[2]
//             let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//             let time=fromto[0]+':'+fromto[1]+fromto[2]
//             doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"orthopedic",booker:send.email,bookerbday:send.birthday,timebooked:time})

//           }

//         }
//         {

//         }
//             }
//        localStorage.doctorchoosed=JSON.stringify(doctorchoosed)
//        location.reload()
//       }
//       else alert("You have already booked this doctor")
//   }
//   else alert ("You must choose a day")

//       }
//     }
//     }
//   }

// }
// else if (location.pathname.includes("brain")){

//   for(j of neurologydata){
//     if ( nameofdoc.toUpperCase().includes(j.name.toUpperCase())){
//     for(k of day){
//       if (k.selected==1){
//   if(k.value!="null"){
//     let pu=0;
//     for (m of doctorchoosed){
//       if (m.doctoremail==j.email&&m.booker==send.email){pu=1;break;}
//     }
//     if (!pu){
//       if (doctorchoosed.length==0){
//         let avail=e.target.parentElement.parentElement.children[1].children[2]
//         let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//         let time=fromto[0]+':'+fromto[1]+fromto[2]
//               doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"neurology",booker:send.email,bookerbday:send.birthday,timebooked:time})
//             }
//             else{
//         let objs=[];
//         for (M of doctorchoosed){
//         if(M.department=="neurology"){
//         objs.push(M)
//         }
//         }
//         if (objs.length==0){
//           let avail=e.target.parentElement.parentElement.children[1].children[2]
//         let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//         let time=fromto[0]+':'+fromto[1]+fromto[2]
//               doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"neurology",booker:send.email,bookerbday:send.birthday,timebooked:time})
//         }
//         else{

//           // let lastele=objs[objs.length-1].timebooked
//           //********************************************** */
//           let lastele=[];
//           for (P of objs){
//             if (P.daybooked==k.value){
//         lastele.push(P)
//             }
//           }
//           if (lastele.length!=0){
//             lastele=lastele[lastele.length-1].timebooked

//             let avail=e.target.parentElement.parentElement.children[1].children[2]
//             let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//             let time=fromto[3]+':'+fromto[4]+fromto[5]
//           let inc=incrementtime(lastele,time)
//           if (inc==-1){
//             alert("This day has filled with patients")
//             return;
//           }
//           else{
//             doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"neurology",booker:send.email,bookerbday:send.birthday,timebooked:inc})

//           }
//           }
//           else{
//             let avail=e.target.parentElement.parentElement.children[1].children[2]
//             let fromto=avail.innerText.match(/\d+|(p|a)m/ig)
//             let time=fromto[0]+':'+fromto[1]+fromto[2]
//             doctorchoosed.push({bookername:send.name,doctorname:j.name,doctoremail:j.email,daybooked:k.value ,department:"neurology",booker:send.email,bookerbday:send.birthday,timebooked:time})

//           }

//         }
//         {

//         }
//             }
//        localStorage.doctorchoosed=JSON.stringify(doctorchoosed)
//        location.reload()
//     }
//     else alert("You have already booked this doctor")
//   }
//   else alert ("You must choose a day")

//       }
//     }
//     }
//   }

// }

// }
// }
