let tog=document.querySelector('.tog');
let show1=document.querySelector('#check')
let show2=document.querySelector('#showpass')
let passInp=document.querySelectorAll('input[type="password"]')
let modalfade=document.querySelectorAll('.modal.fade');
let bookbtn=document.querySelector('.book');
let submit=document.querySelectorAll('input[type=submit]')
let nul=document.querySelectorAll('option[value=null]')
onclick=(e)=>{
if(e.target.className!='d-none'&&e.target.tagName!='SELECT'){
for (i of nul){
  i.selected=1;
  
    
}
   
}


}
onscroll=()=>{
if(scrollY>65){
    tog.style.display='flex';
}
else{
    tog.style.display='none';
}

}
// show1.onchange=()=>{
//     passInp[0].type=passInp[0].type=='password'?'text':'password';
// }
// show2.onchange=()=>{
//     passInp[1].type=passInp[1].type=='password'?'text':'password';
// }