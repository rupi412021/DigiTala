$(function () {
    if (localStorage["UserName"] != null) {
        ajaxCall("GET", "../api/Teachers", "", getUserSuccess, getUserError);
    }

    else {
        location.replace("sign-in.html");
    }

    ajaxCall("GET", "../api/Students", "", getStudentSuccess, getStudentError)
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
        }

    }

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
        this.parentNode.classList.add("active");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id == eleId)
                arr[i].setAttribute("style", "display: block;");
        }
    }

    localStorage.setItem("StudentID", eleId);

    for (var i = 0; i < stundetlist.length; i++) {
        if (stundetlist[i].StudentId == eleId) {
            let fName = stundetlist[i].SFirstName;
            let lName = stundetlist[i].SLastName;
            let fullName = fName + " " + lName;
            localStorage.setItem("StudentFName", fullName);
        }
    }
});

$(document).on("click", ".openTalaManually", function () {
    CurrentStudentId = this.parentNode.parentNode.getAttribute('id');
    arr = document.getElementsByClassName("openTalaManually");
    for (var i = 0; i < arr.length; i++) {
        arr[i].parentNode.classList.remove("active");
        arr[i].classList.remove("toggled");
    }
    this.parentNode.classList.add("active");
    this.classList.add("toggled");

    if (document.URL.includes("StudentTala.html"))
        event.preventDefault();
});

$(document).on("click", ".openProfileManually", function () {
    CurrentStudentId = this.parentNode.parentNode.getAttribute('id');
    arr = document.getElementsByClassName("openProfileManually");
    for (var i = 0; i < arr.length; i++) {
        arr[i].parentNode.classList.remove("active");
        arr[i].classList.remove("toggled");
    }
    this.parentNode.classList.add("active");
    this.classList.add("toggled");

    if (document.URL.includes("StudentProfile.html"))
        event.preventDefault();
});



 