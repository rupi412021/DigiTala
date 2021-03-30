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

$(document).on("click", ".openManually", function () {
    eleId = this.getAttribute('id');
    arr = document.getElementsByClassName("ml-menu");

    if ($("#" + eleId).hasClass("toggled")) {
        document.getElementById(eleId).classList.remove("toggled");
        document.getElementById(eleId).parentNode.classList.remove("active");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id == eleId) {
                arr[i].setAttribute("style", "display: none;");
            }
        }
    }

    else {
        document.getElementById(eleId).classList.add("toggled");
        document.getElementById(eleId).parentNode.classList.add("active");
        for (var i = 0; i < arr.length; i++) {
            if (arr[i].id == eleId)
                arr[i].setAttribute("style", "display: block;");
        }
    }
});

function getStudentSuccess(Students) {
    str = '';

    for (var i = 0; i < Students.length; i++) {
        str += '<li><a href="javascript:void(0);" class="menu-toggle waves-effect waves-block openManually" id=' + Students[i].StudentId +'><span>' + Students[i].SFirstName + ' ' + Students[i].SLastName + "</span></a>" +
            '<ul class="ml-menu" id=' + Students[i].StudentId +'><li><a href="StudentProfile.html">תיק אישי</a></li><li><a href="StudentTala.html">תל"א</a></li></ul></li>';
    }

    $("#renderStudentsinMenu").html(str);
}

 