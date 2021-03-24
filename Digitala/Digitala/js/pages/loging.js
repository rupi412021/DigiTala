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

    for (var i = 0; i < Students.length; i++) {
        str += '<li><a class="menu-toggle"><span>' + Students[i].SFirstName + ' ' + Students[i].SLastName + '</span></a>' +
            '<ul class="ml-menu"><li><a href="StudentProfile.html">תיק אישי</a></li><li><a href="StudentTala.html">תל"א</a></li></ul></li>';
    }

    $("#renderStudentsinMenu").html(str);
    console.log(str);
}


           
