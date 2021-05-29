$(function () {
    if (localStorage["UserName"] != null) {
        ajaxCall("GET", "../api/Teachers", "", getUserSuccess, getUserError);
    }

    else {
        location.replace("sign-in.html");
    }

    studentsWithTala = [];
});

function getUserSuccess(data) {
    user = localStorage["UserName"];
    isadmin = false;
    tEmail = "";
    tname = "";

    for (var i = 0; i < data.length; i++) {
        if (data[i].TeacherEmail == user) { 
            isadmin = data[i].TeacherAdmin;
            tEmail = data[i].TeacherEmail;
            tname = data[i].TeacherFname;
            tyear = data[i].TeacherYear;
            tclass = data[i].TeacherClass;           

            ajaxCall("GET", "../api/Students/" + data[i].TeacherID + "/" + tyear , "", getStudentSuccess, getStudentError);
        }
        
    }

    localStorage.setItem("TeacherYear", tyear);
    localStorage.setItem("TeacherClass", tclass);

    str = "<div class='name' data-toggle='dropdown' aria-haspopup='true' aria-expanded='false'> שלום " + tname + " </div>" +
        "<div class='email' >" + tEmail + "</div>";
    $("#signin").html(str);

    if (isadmin == true) {
        $("#adminPage").css("display", "block");
    }
    else {
        $("#adminPage").css("display", "none");
    }
}

function getUserError(error) {
    console.log(error);
}

function getStudentError(error) {
    console.log(error);
}

function getStudentSuccess(Students) {
    str = '';
    stundetlist = Students;
    for (var i = 0; i < Students.length; i++) {
        str += '<li><a href="javascript:void(0);" class="menu-toggle waves-effect waves-block openStudentManually" id=' + Students[i].StudentId + '><span>' + Students[i].SFirstName + ' ' + Students[i].SLastName + "</span></a>" +
            '<ul class="ml-menu" id=' + Students[i].StudentId + '><li><a class="waves-effect waves-block openProfileManually" href="StudentProfile.html">תיק אישי</a></li><li><a class="waves-effect waves-block openTalaManually" href="StudentTala.html">תל"א</a></li></ul></li>';
    }

    $("#renderStudentsinMenu").html(str);
}

$(document).on("click", ".openStudentManually", function () {
    eleId = this.getAttribute('id');
    arr = document.getElementsByClassName("ml-menu");

    if ($("#" + eleId).hasClass("toggled")) {
        this.classList.remove("toggled");
        this.parentNode.classList.remove("active");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id == eleId) {
                arr[i].setAttribute("style", "display: none;");
            }
        }
    }

    else {
        this.classList.add("toggled");

        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id == eleId)
                arr[i].setAttribute("style", "display: block;");
        }
    }

  
});

$(document).on("click", ".openTalaManually", function () {
    CurrentStudentId = this.parentNode.parentNode.getAttribute('id');
    toggle_actice_menus();
    this.parentNode.parentNode.parentNode.classList.add("active");
    this.classList.add("toggled");
    if (newID == CurrentStudentId && window.location.pathname == "/pages/StudentTala.html") {
        event.preventDefault();
    }
    else {
        arr = document.getElementsByClassName("openStudentManually");
        newID = this.parentNode.parentNode.getAttribute('id');
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id == newID) {
                studentName = arr[i].innerText;
                $("#studentName").html(studentName);
            }
            localStorage.setItem("StudentName", studentName);
            localStorage.setItem("StudentID", newID);
        }
    }
});

$(document).on("click", ".openProfileManually", function () {
    CurrentStudentId = this.parentNode.parentNode.getAttribute('id');    
    toggle_actice_menus();
    this.parentNode.parentNode.parentNode.classList.add("active");
    this.classList.add("toggled");
    if (newID == CurrentStudentId && window.location.pathname == "/pages/StudentProfile.html") {
        event.preventDefault();
    }
    else {
        arr = document.getElementsByClassName("openStudentManually");
        newID = this.parentNode.parentNode.getAttribute('id');
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id == newID) {
                studentName = arr[i].innerText;
                $("#studentName").html(studentName);
            }
            localStorage.setItem("StudentName", studentName);
            localStorage.setItem("StudentID", newID);
        }
    }
});

function toggle_actice_menus() {
    arr = document.getElementsByClassName("openProfileManually");
    arr2 = document.getElementsByClassName("openTalaManually");
    for (var i = 0; i < arr.length; i++) {
        arr[i].parentNode.classList.remove("active");
        arr[i].classList.remove("toggled");
    }
    for (var i = 0; i < arr2.length; i++) {
        arr2[i].parentNode.classList.remove("active");
        arr2[i].classList.remove("toggled");
    }


    studentName = localStorage["StudentName"];
    newID = localStorage["StudentID"]
    $("#studentName").html(studentName);
}

