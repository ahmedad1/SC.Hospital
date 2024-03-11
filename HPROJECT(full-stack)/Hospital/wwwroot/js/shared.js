export const backendOrigin=location.origin+"/api/Account/"
export function getCookie(key){
    let cookies=document.cookie;
    let KVPairs=cookies.split("; ")
    
    for(let chunk of KVPairs){
        let pair=chunk.split("=")
        if(pair[0]==key)
            return decodeURIComponent(pair[1])
    }
    return null;
    
}
export function setCookie(name, value, days) {
    var expires = "";
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        expires = "; expires=" + date.toUTCString();
    }
    document.cookie = name + "=" + encodeURIComponent(value) +"; " + expires + "; path=/; SameSite=Strict; Secure=true;";
}

export async function postJSON(url,body){
        return await fetch(url,{method:"POST",
                  headers:{"content-type":"application/json"},
                  body:JSON.stringify(body)
                });
}
export async function DeleteRequest(url){
return await fetch (url,{
    method:"DELETE"
  
})
}
export function deleteAllCookies(){
    setCookie("email"," ",-5)
    setCookie("userName"," ",-5)
    setCookie("firstName"," ",-5)
    setCookie("lastName"," ",-5)
    setCookie("role"," ",-5)
    setCookie("birthDate"," ",-5)
    setCookie("gender"," ",-5)

}
export async function signOut(){
    let result=await DeleteRequest(`${backendOrigin}SignOut`)
    if(result.status==200)
    location.href=`${location.origin}/index.html`
}
export async function checkForCookies(){
    
        if(!(getCookie("role")
        &&getCookie("userName")
        &&getCookie("firstName")
        &&getCookie("lastName")
        &&getCookie("gender")
        &&getCookie("birthDate")
        &&getCookie("email")
        )
        ){
          
         await signOut();
       
          
      
        }
      
      
}
export function getTextFromHtml(str){
    let map={
        '<':'&lt;',
        '>':'&gt;',
    }
    let result=str.replace(/[<>]/ig,function(e){return map[e]})
     return result

}
export function getNavAfterLogin(name){
    return `<a href="user.html" class="navbar-brand text-primary">S.C Hospital</a><!--special care hospital-->
    <!-- <input type="text"placeholder=search class=" d-lg-none mr-4 "style=width:50%;outline:none;> -->
    <a href="#N"data-toggle=collapse class="navbar-toggler"><span class="navbar-toggler-icon"></span></a>
    
    <div class="collapse navbar-collapse" id="N">
        <ul class="navbar-nav ml-auto ">
            <li class="nav-item"><a href="user.html" class="nav-link">Home</a></li>
            <li class="nav-item"><a href="user.html#serv" class="nav-link">Services</a></li>
            <li class="nav-item"><a href="user.html#contact" class="nav-link">Contact us</a></li>
            <li class="nav-item"><a href="user.html#about" class="nav-link">About us</a></li>
            <li class="nav-item"><a href="user.html#feed" class="nav-link">Feedback</a></li>
            <li class="nav-item"><a href="#appo" class="nav-link appoints">Appointments</a></li>
            <li class="nav-item"><a href="./changepassword.html" class="nav-link">Password</a></li>
            <!-- <li class="nav-item"><a href="#" class="nav-link btn btn-outline-primary text-primary rounded">Login</a></li>-->
        
        </ul>
        <!-- <a href="#login" class="nav-link btn btn-outline-primary ml-lg-2 mt-sm-3 mt-lg-0  "style=max-width:78px>Login</a> -->
       <div class="nav-link d-flex p-0 mt-lg-0 mt-3"style="gap:5px">
        <img src="./images/user-solid.svg"class="ml-lg-2  mt-lg-0  "width=15 alt="">
        <span class="text-muted usernamespan">${getTextFromHtml(name)}</span>
       
        </div> 
    <button class=" btn btn-outline-primary ml-lg-3 mt-3 mt-lg-0 signout">Sign out</button>
    </div>`
}
