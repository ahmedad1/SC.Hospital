import { signOut } from "./shared.js";

let signOutBtn = document.querySelector(".signout");
signOutBtn.addEventListener("click", signOut);
let usernamespan = document.querySelector(".usernamespan");
usernamespan.innerText = getCookie("firstName");
