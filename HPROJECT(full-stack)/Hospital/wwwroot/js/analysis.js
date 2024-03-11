let analysisreq=localStorage.analysis==undefined?[]:JSON.parse(localStorage.analysis)
let sendusr=JSON.parse(localStorage.send)
let tbody=document.querySelector('tbody')
if (analysisreq.length!=0){
for (i=0;i<analysisreq.length ;i++){
if (sendusr.email==analysisreq[i].patientemail){
    
    for (j=0;j<analysisreq[i].requiredanalysis.length;j++){
        let lengthofparent=tbody.children.length
let tabl=`<tr>

<th scope="col">${lengthofparent+1}</th>
<td scope="col">${analysisreq[i].doctorname}</td>
<td scope="col">${analysisreq[i].department}</td>
<td scope="col">${analysisreq[i].requiredanalysis[j]}</td>


</tr>`
tbody.innerHTML+=tabl;
    }

}

}


}