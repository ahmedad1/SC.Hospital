import { AddDoctorTable, AddPatientTable, UpdateTokens, backendAccountApi, checkForCookies, getCookie, signOut } from "./shared.js"

let patientInSidebar=document.querySelector('.patient-sidebar')
let doctorInSidebar=document.querySelector('.doctor-sidebar')
let section=document.querySelector("section")
let selects=document.querySelectorAll("select")
let usernamespan=document.querySelector('.usernamespan')
let signOutBtn=document.querySelector(".signout")
usernamespan.innerText=getCookie("firstName")
signOutBtn.addEventListener("click",signOut)
let fetchRenderPatients=async(replaceDoctor=false)=>{
    let data=[]
    const result=await fetch(`${backendAccountApi}patients`,{
        method:"POST",
        headers:{
        "content-type":"Application/Json"
        },
        body:JSON.stringify(sessionStorage.getItem("page"))

    }

);
if(result.status!=200){
 if(getCookie("role")=="Adm"){
     await UpdateTokens()
     const result=await fetch(`${backendAccountApi}patients`,{
        method:"POST",
        headers:{
        "content-type":"Application/Json"
        },
        body:JSON.stringify(sessionStorage.getItem("page"))

    });
    data=await result.json()
 }   
}
else{
    data=await result.json()

}
AddPatientTable(section,data,replaceDoctor)
}
onload=async()=>{
    await checkForCookies()
    sessionStorage.setItem("page",1)
    fetchRenderPatients()
}
selects.forEach(e=>{
    e.addEventListener("change",event=>{
       
            for(let i of event.target.parentElement.parentElement.children){
                if(i.nodeName!="TD"||i.children[0].nodeName!="INPUT"){
               
                continue
                }

                i.children[0].disabled= event.target.value!="update"
            }
        
    })
})
window.addEventListener("scroll",async ()=>{
    if(patientInSidebar.classList.contains("active"))
    if(window.innerHeight+window.scrollY>=document.body.offsetHeight){ //reached end of scroll
        sessionStorage.setItem("page",+sessionStorage.getItem("page")+1)
        await fetchRenderPatients()
    }
})
patientInSidebar.addEventListener("click",_=>{
    if(patientInSidebar.classList.contains("active"))
    return
    doctorInSidebar.classList.remove("active")
    doctorInSidebar.classList.add("bg-light")
    patientInSidebar.classList.add("active")
    patientInSidebar.classList.remove("bg-light")
    /*
     <th scope="col">Id</th>
                <th scope="col">First Name</th>
                <th scope="col">Last Name</th>
                <th scope="col">User Name</th>
                <th scope="col">Email</th>
                <th scope="col">Gender</th>
                <th scope="col">BirthDate</th>
                <th scope="col">EmailConfirmed</th>
                <th scope="col">Options</th>
                <th scope="col">Commit</th>
    
    */
    fetchRenderPatients(true)
    
})
doctorInSidebar.addEventListener("click",_=>{
    if(doctorInSidebar.classList.contains("active"))
    return
    patientInSidebar.classList.remove("active")
    patientInSidebar.classList.add("bg-light")
    doctorInSidebar.classList.remove("bg-light")
    doctorInSidebar.classList.add("active")
    AddDoctorTable(section,[],true)
})
