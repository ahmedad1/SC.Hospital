import { backendAccountApi, fetchJSONAuth, signOut } from "./shared.js";

let signOutBtn = document.querySelector(".signout");
signOutBtn.addEventListener("click", signOut);
let usernamespan = document.querySelector(".usernamespan");
usernamespan.innerText = getCookie("firstName");
const departmentSelect = document.getElementById("departmentName");
const addDoctorBtn = document.getElementById("addDoctor");
addDoctorBtn.addEventListener("click", async (e) => {
  e.preventDefault();
  const result = await fetchJSONAuth(
    `${backendAccountApi}Departments/no-verbos`,
    {},
    "GET"
  );
  result.forEach((d) => {
    departmentSelect.insertAdjacentHTML(
      "beforeend",
      `<option value='${d}'selected>${d}</option> `
    );
  });
});
