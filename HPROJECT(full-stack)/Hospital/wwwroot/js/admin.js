import {
  AddDoctorTable,
  AddPatientTable,
  UpdateTokens,
  appendLoadingIcon,
  backendAccountApi,
  checkForCookies,
  fetchJSONAuth,
  getCookie,
  postJSON,
  signOut,
} from "./shared.js";

let patientInSidebar = document.querySelector(".patient-sidebar");
let doctorInSidebar = document.querySelector(".doctor-sidebar");
let section = document.querySelector("section");
let selects = document.querySelectorAll("select");
let usernamespan = document.querySelector(".usernamespan");
let signOutBtn = document.querySelector(".signout");
let searchForm = document.querySelector(".searchForm");
let searchType = document.getElementById("searchType");
let searchValue = document.querySelector(".searchValue");
usernamespan.innerText = getCookie("firstName");
signOutBtn.addEventListener("click", signOut);
let isSearchingGlobal = 0;
let searchingValueGlobal;
let searchingTypeGlobal;
let currentSection = "patient";
let addLoadingInSection = () => {
  section.insertAdjacentHTML(
    "beforeend",
    "<h1 class='p-5 text-muted'><img src='images/loading.png'class='loading' alt=''class='ml-2'> Loading....</h1>"
  );
};
let fetchRenderPatients = async (replaceDoctor = false) => {
  let data = [];
  if (replaceDoctor) addLoadingInSection();
  const result = await fetch(`${backendAccountApi}patients`, {
    method: "POST",
    headers: {
      "content-type": "Application/Json",
    },
    body: JSON.stringify(sessionStorage.getItem("page")),
  });
  if (result.status != 200) {
    if (getCookie("role") == "Adm") {
      await UpdateTokens();
      const result = await fetch(`${backendAccountApi}patients`, {
        method: "POST",
        headers: {
          "content-type": "Application/Json",
        },
        body: JSON.stringify(sessionStorage.getItem("page")),
      });
      data = await result.json();
    }
  } else {
    data = await result.json();
  }

  AddPatientTable(section, data, replaceDoctor);
  if (data.length == 0) return false;
  return true;
};
window.addEventListener("DOMContentLoaded",async function(){
  await checkForCookies();
})
onload = async() => {
  
  sessionStorage.setItem("page", 1);
  await fetchRenderPatients(true);
};
selects.forEach((e) => {
  e.addEventListener("change", (event) => {
    for (let i of event.target.parentElement.parentElement.children) {
      if (i.nodeName != "TD" || i.children[0].nodeName != "INPUT") {
        continue;
      }

      i.children[0].disabled = event.target.value != "update";
    }
  });
});
window.addEventListener("scroll", async () => {
  if (patientInSidebar.classList.contains("active"))
    if (window.innerHeight + window.scrollY >= document.body.offsetHeight) {
      //reached end of scroll
      sessionStorage.setItem("page", +sessionStorage.getItem("page") + 1);
      let result;
      if (!isSearchingGlobal) {
        result = await fetchRenderPatients();
      } else {
        result = await searchHandler(sessionStorage.getItem("page"));
      }
      if (!result)
        sessionStorage.setItem("page", +sessionStorage.getItem("page") - 1);
    }
});
patientInSidebar.addEventListener("click", (_) => {
  isSearchingGlobal = 0;
  searchingTypeGlobal = undefined;
  searchingValueGlobal = undefined;
  if (!patientInSidebar.classList.contains("active")) {
    currentSection = "patient";
    doctorInSidebar.classList.remove("active");
    doctorInSidebar.classList.add("bg-light");
    patientInSidebar.classList.add("active");
    patientInSidebar.classList.remove("bg-light");
  }
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
  sessionStorage.setItem("page", 1);
  fetchRenderPatients(true);
});

async function searchHandler(pageNum) {
  if (pageNum == 1) addLoadingInSection();
  if (searchingTypeGlobal == "EmailConfirmed" && !isNaN(searchingValueGlobal))
    searchingValueGlobal = searchingValueGlobal == "0" ? "false" : "true";
  let result = await fetchJSONAuth(
    `${backendAccountApi}patients/${searchingTypeGlobal}`,
    { data: searchingValueGlobal, page: pageNum },
    "POST"
  );

  AddPatientTable(section, result, pageNum == 1);
  return result.length != 0;
}
searchForm.addEventListener("submit", async (e) => {
  e.preventDefault();
  isSearchingGlobal = 1;
  sessionStorage.setItem("page", 1);
  searchingValueGlobal = searchValue.value;
  searchingTypeGlobal = searchType.value;
  await searchHandler(1);
});

async function fetchRenderDoctors(replacePatient) {
  let doctors = await fetchJSONAuth(
    `${backendAccountApi}doctors`,
    { page: +sessionStorage.getItem("page") },
    "POST"
  );
  if (typeof doctors !== "number") {
    AddDoctorTable(section, doctors, replacePatient);
    return true;
  }
  return false;
}
doctorInSidebar.addEventListener("click", async (_) => {
  isSearchingGlobal = 0;
  searchingTypeGlobal = undefined;
  searchingValueGlobal = undefined;
  if (!doctorInSidebar.classList.contains("active")) {
    patientInSidebar.classList.remove("active");
    patientInSidebar.classList.add("bg-light");
    doctorInSidebar.classList.remove("bg-light");
    doctorInSidebar.classList.add("active");
    currentSection = "doctor";
  }

  sessionStorage.setItem("page", 1);

  await fetchRenderDoctors(true);
});
