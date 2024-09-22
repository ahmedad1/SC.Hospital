let inp=document.querySelector('input[name=phone]')
let reg=/[^0-9]/ig
inp.onkeypress=(e)=>{
if(e.key.match(reg)){
    e.preventDefault();
}


}