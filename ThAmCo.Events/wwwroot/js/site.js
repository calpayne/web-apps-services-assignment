function updateAttendance(customerId, eventId) {
    $.ajax({
        url: "http://localhost:22263/GuestBookings/UpdateAttendance/",
        method: "POST",
        data: "customerId=" + customerId + "&eventId=" + eventId,
        error: function (data) {
            alert("Error attendance couldn't be updated.");
        }
    });
}

$(document).ready(function () {
    $('.table').DataTable({
        'aoColumnDefs': [{
            'bSortable': false,
            'aTargets': [-1]
        }]
    });
});