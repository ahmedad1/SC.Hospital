const backendOrigin = location.origin;
export const backendAccountApi = backendOrigin + "/api/Account/";
const loadingIcon =
  '<img src="images/loading.png"class="loading" alt=""class="ml-2">';

export function getCookie(key) {
  let cookies = document.cookie;
  let KVPairs = cookies.split("; ");

  for (let chunk of KVPairs) {
    let pair = chunk.split("=");
    if (pair[0] == key) return decodeURIComponent(pair[1]);
  }
  return null;
}
export function getRequiredDateFormat(date) {
  //yyyy-MM-dd
  //8/4/2002
  if (/\d{4}-\d{2}-\d{2}/gi.test(date)) return date;
  let splittedDate = date.split("/");
  let month = splittedDate[0];
  let day = splittedDate[1];
  let year = splittedDate[2];
  month = month.padStart(2, "0");
  day = day.padStart(2, "0");
  return year + "-" + month + "-" + day;
}
export function setCookie(name, value, days) {
  var expires = "";
  if (days) {
    var date = new Date();
    date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
    expires = "; expires=" + date.toUTCString();
  }
  document.cookie =
    name +
    "=" +
    encodeURIComponent(value) +
    "; " +
    expires +
    "; path=/; SameSite=Strict; Secure=true;";
}

export async function postJSON(url, body) {
  return await fetch(url, {
    method: "POST",
    headers: { "content-type": "application/json" },
    body: JSON.stringify(body),
  });
}

export async function fetchJSONAuth(url, body, methodName) {
  const result = await fetch(url, {
    method: methodName,
    headers: { "content-type": "application/json" },
    body: JSON.stringify(body),
  });
  if (result.status == 401) {
    await UpdateTokens();
    return await fetchJSONAuth(url, body, methodName);
  }
  try {
    const resultBody = await result.json();
    return resultBody;
  } catch {
    return result.status;
  }
}
export async function patchJSON(url, body) {
  return await fetch(url, {
    method: "PATCH",
    headers: {
      "content-type": "application/json",
    },
    body: JSON.stringify(body),
  });
}
export async function DeleteRequest(url) {
  return await fetch(url, {
    method: "DELETE",
  });
}
export function deleteAllCookies() {
  setCookie("email", " ", -5);
  setCookie("userName", " ", -5);
  setCookie("firstName", " ", -5);
  setCookie("lastName", " ", -5);
  setCookie("role", " ", -5);
  setCookie("birthDate", " ", -5);
  setCookie("gender", " ", -5);
}
export async function signOut(isAllowedToAppendLoadingIcon = true,isAllowedToNavigate=true) {
  if (isAllowedToAppendLoadingIcon) {
    let signOutBtn = document.querySelector(".signout");
    appendLoadingIcon(signOutBtn);
  }
 await DeleteRequest(`${backendAccountApi}sign-out`);
  deleteAllCookies()
  if(isAllowedToNavigate)
  location.href = `${location.origin}/index.html`;
}
export async function checkForCookies(isAllowedToNavigate = true) {
  if (
    !(
      getCookie("role") &&
      getCookie("userName") &&
      getCookie("firstName") &&
      getCookie("lastName") &&
      getCookie("gender") &&
      getCookie("birthDate") &&
      getCookie("email")
    )||
    (getCookie("role")=="Pat"&&(location.pathname.startsWith("/doctor.html")||location.pathname.startsWith("/admin.html")||location.pathname.startsWith("/add-doctor.html")))||
    (getCookie("role")=="Doc"&&!(location.pathname.startsWith("/doctor.html")||location.pathname.startsWith("/ProfileSettings.html")))||
    (getCookie("role")=="Adm"&&!(location.pathname.startsWith("/admin.html")||location.pathname.startsWith("/add-doctor.html")||location.pathname.startsWith("/ProfileSettings.html")))
    
  ) {
    await signOut(false,isAllowedToNavigate);
    if (isAllowedToNavigate) location.href = "/index.html";

    return false;
  }
  return true;
}
export async function UpdateTokens() {
  let result = await postJSON(`${backendAccountApi}tokens`);
  if (result.status == 200) return result.status;
  deleteAllCookies();
  location.href = "/index.html";
}
export function getTextFromHtml(str) {
  let map = {
    "<": "&lt;",
    ">": "&gt;",
  };
  let result = str.replace(/[<>]/gi, function (e) {
    return map[e];
  });
  return result;
}
export function getNavAfterLogin(name) {
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
        <a href="ProfileSettings.html" class="text-muted usernamespan">${getTextFromHtml(
          name
        )}</a>
       
        </div> 
    <button class=" btn btn-outline-primary ml-lg-3 mt-3 mt-lg-0 signout">Sign out</button>
    </div>`;
}
export function changeHrefOfHomeBreadCrumb(url) {
  document.querySelector(".breadcrumb-item a").href = url;
}
export function getNavAfterLoginAdmin(name) {
  return `<a href="admin.html" class="navbar-brand text-primary">S.C Hospital</a><!--special care hospital-->
    <!-- <input type="text"placeholder=search class=" d-lg-none mr-4 "style=width:50%;outline:none;> -->
    <a href="#N"data-toggle=collapse class="navbar-toggler"><span class="navbar-toggler-icon"></span></a>
    
    <div class="collapse navbar-collapse" id="N">
        <ul class="navbar-nav ml-auto ">
        </ul>
        <!-- <a href="#login" class="nav-link btn btn-outline-primary ml-lg-2 mt-sm-3 mt-lg-0  "style=max-width:78px>Login</a> -->
       <div class="nav-link d-flex p-0 mt-lg-0 mt-3"style="gap:5px">
        <img src="./images/user-solid.svg"class="ml-lg-2  mt-lg-0  "width=15 alt="">
        <a href="ProfileSettings.html" class="text-muted usernamespan">${getTextFromHtml(
          name
        )}</a>
       
        </div> 
    <button class=" btn btn-outline-primary ml-lg-3 mt-3 mt-lg-0 signout">Sign out</button>
    </div>`;
}

export function DisplayAlertModal(str, color = "text-danger") {
  let textWithoutSpaces = getTextFromHtml(str).split(" ").join("");

  if (
    !document.body.innerHTML
      .slice(25)
      .includes(`<div class="modal fade ${textWithoutSpaces}`)
  ) {
    document.body.insertAdjacentHTML(
      "beforeend",
      `<div class="modal fade ${textWithoutSpaces}" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
            <h5 class="modal-title text-muted" id="exampleModalLabel">Alert</h5>
            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
            </div>
            <div class="modal-body ${color}"style="">
            ${getTextFromHtml(str)}
            </div>
            <div class="modal-footer">
            <button type="button" class="btn btn-secondary closemodal" data-dismiss="modal">Close</button>
            
            </div>
        </div>
        </div>
        </div>`
    );
  }
  let curModalHTML = document.querySelector(`.${textWithoutSpaces}`);
  let newModal = new bootstrap.Modal(curModalHTML);
  newModal.show();
}
export function appendLoadingIcon(src) {
  src.insertAdjacentHTML("beforeend", loadingIcon);
}
export function removeLoadingIcon(src) {
  if (src.children.length) src.children[0].remove();
}

let toggleDisableInput = (event) => {
  for (let i of event.target.parentElement.parentElement.children) {
    if (
      i.nodeName != "TD" ||
      (i.children[0].nodeName != "INPUT" &&
        i.children[0].nodeName != "SELECT") ||
      i.children[0].classList.contains("manip")
    ) {
      continue;
    }

    i.children[0].disabled = event.target.value != "update";
  }
};

export function AddDoctorTable(section, json, changeFromPatient = false) {
  if (changeFromPatient) {
    section.innerHTML = "";
    section.insertAdjacentHTML(
      "beforeend",
      `
    <a href="../add-doctor.html"target=_blank class="btn btn-info form-control">Add Doctor</a>
       
        <table class="table ">
            
            <thead class="bg-primary text-light">
                
              <tr>
                <th scope="col">Id</th>
                <th scope="col">First Name</th>
                <th scope="col">Last Name</th>
                <th scope="col">User Name</th>
                <th scope="col">Email</th>
                <th scope="col">Gender</th>
                <th scope="col">BirthDate</th>
                <th scope="col">EmailConfirmed</th>
                <th scope="col">Department</th>
                <th scope="col">Schedule</th>
                <th scope="col">Options</th>
                <th scope="col">Commit</th>
               
              </tr>
            </thead>
            <tbody>
             
              
            </tbody>
         
            </table>
    
    `
    );
  }
  if (json.length == 0) return;
  let tbody = document.querySelector("tbody");
  for (let i of json)
    tbody.insertAdjacentHTML(
      "beforeend",
      `
    <tr>
    <th scope="row">${i.id}</th>
    <td><input class="input-cell"type="text" value="${
      i.firstName
    }"disabled></td>
    <td><input class="input-cell"type="text" value="${i.lastName}"disabled></td>
    <td><input class="input-cell"type="text" value="${i.userName}"disabled></td>
    <td><input class="input-cell"type="email" value="${i.email}"disabled></td>
    <td><select name="options" class="form-control">
        <option value="${i.gender == "Male" ? "Male" : "Female"}">${
        i.gender == "Male" ? "Male" : "Female"
      }</option>
        <option value=${i.gender == "Male" ? "Female" : "Male"}>${
        i.gender == "Male" ? "Female" : "Male"
      }</option>
    </select></td>
    <td><input class="input-cell"type="date" value="${getRequiredDateFormat(
      i.birthDate
    )}"disabled></td>
    <td><input class="input-cell"type="text" value="Mark"disabled>${
      i.emailConfirmed
    }</td>
    <td><input class="input-cell"type="text" value="Mark"disabled>${
      i.departmentName
    }</td>
    <td><a class="input-cell"type="text" href="#">Schedule</a></td>
    <td><select name="options" class="form-control manip">
        <option value="delete">Delete</option>
        <option value="update">Update</option>
    </select></td>
    <td ><button class="btn btn-primary saveBtn">Save</button></td>
  </tr>

    `
    );

  let manips = document.querySelectorAll(".manip");
  manips.forEach((e) => {
    e.addEventListener("change", (event) => {
      toggleDisableInput(event);
    });
  });
}
export function AddPatientTable(section, json, changeFromDoctor = false) {
  if (changeFromDoctor) {
    section.innerHTML = "";
    section.insertAdjacentHTML(
      "beforeend",
      `
       
           
            <table class="table ">
                
                <thead class="bg-primary text-light">
                    
                  <tr>
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
                   
                  </tr>
                </thead>
                <tbody>
                 
                  
                </tbody>
             
                </table>
        
        `
    );
  }
  if (json.length == 0) return;
  let tbody = document.querySelector("tbody");
  for (let i of json)
    tbody.insertAdjacentHTML(
      "beforeend",
      `
        <tr>
        <th scope="row">${i.id}</th>
        <td><input class="input-cell"type="text" value="${
          i.firstName
        }"disabled></td>
        <td><input class="input-cell"type="text" value="${
          i.lastName
        }"disabled></td>
        <td><input class="input-cell"type="text" value="${
          i.userName
        }"disabled></td>
        <td><input class="input-cell"type="email" value="${
          i.email
        }"disabled></td>
        <td><select name="options" class="form-control"disabled style="height: 1.85em;padding:0">
            <option value="${i.gender == "Male" ? "Male" : "Female"}">${
        i.gender == "Male" ? "Male" : "Female"
      }</option>
            <option value=${i.gender == "Male" ? "Female" : "Male"}>${
        i.gender == "Male" ? "Female" : "Male"
      }</option>
        </select></td>
        <td><input class="input-cell"type="date" value="${getRequiredDateFormat(
          i.birthDate
        )}"disabled></td>
        <td><input class="input-cell"type="text" value="${
          i.emailConfirmed
        }"disabled></td>
        <td><select name="options" class="form-control manip">
         <option value="delete">Delete</option>
            <option value="update">Update</option>
        </select></td>
        <td ><button class="btn btn-primary saveBtn">Save</button></td>
      </tr>
    
        `
    );
  let manips = document.querySelectorAll(".manip");
  manips.forEach((e) => {
    e.addEventListener("change", (event) => {
      toggleDisableInput(event);
    });
  });
  const saveBtns = document.querySelectorAll(".saveBtn");
  saveBtns.forEach((e) => {
    e.addEventListener("click", async (event) => {
      const parentTr = event.target.parentElement.parentElement;
      const id = parentTr.children[0].innerText;
      let data = [];
      for (let c of parentTr.children) {
        if (c.children[0]) data.push(c.children[0].value);
      }

      if (data[7] == "update") {
        appendLoadingIcon(e);
        if (!isNaN(data[6])) {
          data[6] = data[6] == "0" ? "false" : "true";
        }
        let result = await fetchJSONAuth(
          `${backendAccountApi}user`,
          {
            id: +id,
            firstName: data[0],
            lastName: data[1],
            userName: data[2],
            email: data[3],
            gender: data[4],
            birthDate: data[5],
            EmailConfirmed: data[6],
            role:"patients"
          },
          "PUT"
        );
        if (result.success) {
          DisplayAlertModal("Updated Succesfully", "text-success");
        } else if (result.newUserNameIsExist) {
          DisplayAlertModal("UserName Is Already Exist", "text-danger");
        } else if (result.newEmailIsExist) {
          DisplayAlertModal("Email Is Already Exist", "text-danger");
        } else {
          DisplayAlertModal(
            "Some of the inputs are not in the correct format or something went wrong"
          );
        }
        removeLoadingIcon(e);
      } else if (data[7] == "delete") {
        const confirmModal = document.getElementById("confirmDelete");
        const modal = new bootstrap.Modal(confirmModal);
        modal.show();
        const confirmBtn = document.getElementById("confirmYes");
        confirmBtn.addEventListener("click", async (eventConfirm) => {
          appendLoadingIcon(eventConfirm.target);
          const result = await fetchJSONAuth(
            `https://localhost:7197/api/Account/patients/${id}`,
            {},
            "DELETE"
          );
          removeLoadingIcon(eventConfirm.target);

          if (result != 200) {
            DisplayAlertModal("Something went wrong", "text-danger");
          } else {
            DisplayAlertModal("Deleted Successfully", "text-success");
            parentTr.remove();
          }
        });
      }
    });
  });
}
