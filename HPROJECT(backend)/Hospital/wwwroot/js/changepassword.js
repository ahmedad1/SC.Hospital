let usernamespans=document.querySelector('.usernamespan')
let sends=null
let doctorchooseds=localStorage.doctorchoosed==undefined?[]:JSON.parse(localStorage.doctorchoosed)
sends=JSON.parse(localStorage.send)
usernamespans.innerHTML=sends.name
let dentaldatas=[],ophthalmologydatas=[],internaldatas=[],orthopedicdatas=[],neurologydatas=[]
let doctordatas={};


let appointss=document.querySelector('.appoints')
appointss.onclick=()=>{
  if (doctorchooseds.length==0)
  alert('0 Doctor Booked')
else{
  let daytime=[];
  for(L of doctorchooseds){
if (L.booker==sends.email){
  daytime.push(L)
}
  }
  if(daytime.length==0)alert('0 Doctor Booked')
  else{
    let messages="";
    let X="";
    document.body.innerHTML=`<nav class="navbar navbar-expand-lg bg-light navbar-light sticky-top ">
    <a href="user.html" class="navbar-brand text-primary">S.C Hospital</a><!--special care hospital-->
    <!-- <input type="text"placeholder=search class=" d-lg-none mr-4 "style=width:50%;outline:none;> -->
    <a href="#N"data-toggle=collapse class="navbar-toggler"><span class="navbar-toggler-icon"></span></a>
    
    <div class="collapse navbar-collapse" id="N">
        <ul class="navbar-nav ml-auto ">
            <li class="nav-item"><a href="user.html" class="nav-link">Home</a></li>
            <!-- <li class="nav-item"><a href="#" class="nav-link btn btn-outline-primary text-primary rounded">Login</a></li>-->
        
        </ul>
        <!-- <a href="#login" class="nav-link btn btn-outline-primary ml-lg-2 mt-sm-3 mt-lg-0  "style=max-width:78px>Login</a> -->
       <div class="nav-link d-flex p-0 mt-lg-0 mt-3"style="gap:5px">
        <img src="./images/user-solid.svg"class="ml-lg-2  mt-lg-0  "width=15 alt="">
        <span class="text-muted usernamespan"style=>${sends.name}</span>
       
    </div> 
    <a href="#" class=" btn btn-outline-primary ml-lg-3 mt-3 mt-lg-0 signout appsign">Sign out</a>
    
    
    </div>
    </nav>
    <table class="table sat" id="table">
 
    <thead>
      <tr>
            <th scope="col">Doctor</th>
            <th scope="col">Day</th>
            <th scope="col">Time</th>
            <th scope="col">Department</th>
           
          </tr>
        </thead>
        <tbody>
         
        </tbody>
      </table>
    `
    let appsign=document.querySelector('.appsign');
   appsign.onclick=()=>{
    localStorage.send=null
    location.href='index.html'
   }
    for(Q=0;Q<daytime.length;Q++){
     
  

X=daytime[Q].doctorname;

messages=` 
<tr id="a">
<td> ${X}</td>
<td>${daytime[Q].daybooked}</td>
 <td>${daytime[Q].timebooked}</td>
 <td>${daytime[Q].department}</td>
</tr>`

document.querySelector('tbody').innerHTML+=messages

  
}

  }
}


}


let signouts=document.querySelector('.signout')
signouts.onclick=()=>{
 
  localStorage.send=null
  location.href='index.html'

}
function storedatainvar(){
    doctordatas=JSON.parse(localStorage.doctor)
    dentaldatas=doctordatas.dental
    ophthalmologydatas=doctordatas.ophthalmology
    internaldatas=doctordatas.internal
    orthopedicdatas=doctordatas.orthopedic
    neurologydatas=doctordatas.neurology

}

let patientdatas=JSON.parse(localStorage.patient)
let formpaswd=document.querySelector('.chgpass')
let oldp=document.querySelector('.oldp')
let newp=document.querySelector('.newp')
formpaswd.onsubmit=(e)=>{
e.preventDefault()
if (oldp.value==sends.password){
for (i=0;i<patientdatas.length ;i++)
if (patientdatas[i].email==sends.email){
patientdatas[i].password=newp.value
sends.password=newp.value
localStorage.send=JSON.stringify(sends)
localStorage.patient=JSON.stringify(patientdatas);
location.reload();
}
let deptsearch=sends.department;
if(deptsearch!=undefined){
let doc=JSON.parse(localStorage.doctor)

for(O in doc ){
    if(O==deptsearch){
for (S=0;S<doc[O].length; S++){
    if ((doc[O])[S].email==sends.email){
     (doc[O])[S].password=newp.value
     localStorage.send=JSON.stringify(((doc[O])[S]))
     localStorage.doctor=JSON.stringify(doc)
     location.reload()

    }


}

    }
} 
}

}
else{
    alert("The Old Password Is Incorrect")
}
}