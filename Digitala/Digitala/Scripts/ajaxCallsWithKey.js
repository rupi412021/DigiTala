function ajaxCall(method, api, data, successCB, errorCB) {
    $.ajax({
        type: method,
        url: api,
        data: data,
        cache: false,
		headers: {
            'user-key': '79e7344bc82adecdd9d1efdc9b98b543',
        },
        contentType: "application/json",
        dataType: "json",
        success: successCB,
        error: errorCB
    });
}