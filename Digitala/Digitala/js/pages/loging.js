$(function () {
    if (localStorage["UserName"] != null) {
        ajaxCall("GET", "../api/Teachers", "", getUserSuccess, getUserError);
    }

    else {
        location.replace("sign-in.html");
    }

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
            ajaxCall("GET", "../api/Students/" + data[i].TeacherID + "/2021", "", getStudentSuccess, getStudentError);
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

    for (var i = 0; i < Students.length; i++) {
        ajaxCall("GET", "../api/Talas/" + Students[i].StudentId + "/2021", "", getTalaSuccess, getStudentError);
    }

}

function getTalaSuccess(talas) {
    studentsWithTala = talas;
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

  
});

$(document).on("click", ".openTalaManually", function () {
    newID = localStorage["StudentID"]
    CurrentStudentId = this.parentNode.parentNode.getAttribute('id');
    if (newID == CurrentStudentId && this.parentNode.parentNode.hasClass('active')) {
        event.preventDefault();
    }

    else {
        arr = document.getElementsByClassName("openTalaManually");
        for (var i = 0; i < arr.length; i++) {
            arr[i].parentNode.classList.remove("active");
            arr[i].classList.remove("toggled");
        }
        this.parentNode.classList.add("active");
        this.classList.add("toggled");

        arr = document.getElementsByClassName("openStudentManually");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id == CurrentStudentId) {
                studentName = arr[i].innerText;
                $("#studentName").html(studentName);
            }
            localStorage.setItem("StudentName", studentName);
            localStorage.setItem("StudentID", CurrentStudentId);
        }

        for (var i = 0; i < studentsWithTala.length; i++) {
            if (studentsWithTala[i].StudentId == CurrentStudentId)
                window.location.href("StudentProfile.html")
        }
    }
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

    studentName = localStorage["StudentName"];
    newID = localStorage["StudentID"]
    $("#studentName").html(studentName);

    if (newID == this.parentNode.parentNode.getAttribute('id') && this.parentNode.parentNode.hasClass('active')) {
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
