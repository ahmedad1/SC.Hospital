import { signOut } from "./shared.js";

let signOutBtn = document.querySelector(".signout");
signOutBtn.addEventListener("click", signOut);

// let usernamespan=document.querySelector('.usernamespan')
// let send=null
// let doctorchoosed=localStorage.doctorchoosed==undefined?[]:JSON.parse(localStorage.doctorchoosed)
// send=JSON.parse(localStorage.send)
// usernamespan.innerHTML=send.name

// onload=()=>{
// if (location.hash=='#appo'){
//   appoints.click();
// }
// }
// let appoints=document.querySelector('.appoints')
// appoints.onclick=()=>{
// //   if (doctorchoosed.length==0)
// //   alert('0 Doctor Booked')
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
//     <a href="#" class=" btn btn-outline-primary ml-lg-3 mt-3 mt-lg-0 signout appsign">Sign out</a>
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
//    appsign.onclick=()=>{
//     localStorage.send=null
//     location.href='index.html'
//    }
//     for(Q=0;Q<daytime.length;Q++){

// X=daytime[Q].doctorname;

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

// let signout=document.querySelector('.signout')
// signout.onclick=()=>{

//   localStorage.send=null
//   location.href='index.html'

// }
