import { DisplayAlertModal, backendDepartmentApi, fetchJSONAuth, postMultiPart } from "./shared.js";

const departmentName = document.getElementById("departmentName");
const departmentDiscription = document.getElementById("discription");
const departmentCardImage = document.getElementById("cardImage");
const submitDepartmentForm = document.getElementById("addDepartment");
const departmentForm = document.getElementById("departmentForm");

departmentForm.addEventListener("submit", async (e) => {
  e.preventDefault();
  const formData = new FormData();
  formData.append("departmentName", departmentName.value);
  formData.append("description", departmentDiscription.value);
  formData.append("backgroundCardImage", departmentCardImage.files[0]);
  let result = await postMultiPart(`${backendDepartmentApi}`, formData, "POST");
  if(result.status==200){
  DisplayAlertModal("Added Successfully","text-success")
  departmentForm.reset()
  }


});
