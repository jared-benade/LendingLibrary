var LendingLibrary = LendingLibrary || {};
LendingLibrary.People = LendingLibrary.People || {};
LendingLibrary.People.Index = LendingLibrary.People.Index || {};
(function (ns) {
    ns.deletePerson = function(id, deleteUrl, redirectUrl) {
        $.post(deleteUrl, { id: id }).then(function() {
            sweetAlert({
                    title: 'Person deleted successfully',
                    text: '',
                    type: 'success',
                    confirmButtonColor: '#7CB43F'
                },
                function() {
                    $.redirect(redirectUrl);
                });
        });
    };
})(LendingLibrary.People.Index);