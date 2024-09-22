
async function cFC() {
  if (
    !(
      gC("role") &&
      gC("userName") &&
      gC("firstName") &&
      gC("lastName") &&
      gC("gender") &&
      gC("birthDate") &&
      gC("email")
    )
  ) {
    await sO(false);
    

    return false;
  }
  return true;
}
function gC(key) {
  let cookies = document.cookie;
  let KVPairs = cookies.split("; ");

  for (let chunk of KVPairs) {
    let pair = chunk.split("=");
    if (pair[0] == key) return decodeURIComponent(pair[1]);
  }
  return null;
}

await cFC();
function gTFH(str) {
  let map = {
    "<": "&lt;",
    ">": "&gt;",
  };
  let result = str.replace(/[<>]/gi, function (e) {
    return map[e];
  });
  return result;
}
function gNAL(name) {
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
        <a href="ProfileSettings.html" class="text-muted usernamespan">${gTFH(
          name
        )}</a>
       
        </div> 
    <button class=" btn btn-outline-primary ml-lg-3 mt-3 mt-lg-0 signout">Sign out</button>
    </div>`;
}
async function dR(url) {
  return await fetch(url, {
    method: "DELETE",
  });
}
async function sO() {
  let result = await dR(`${backendAccountApi}sign-out`);
  if (result.status == 200) location.href = `${location.origin}/index.html`;
}
(async function () {
 
    let nav = document.querySelector("nav");
    nav.innerHTML = gNAL(gC("firstName"));
    document.querySelector("a[href='index.html#sign']").parentElement.remove()
   
  
})();
