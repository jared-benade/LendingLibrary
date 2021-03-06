﻿describe('people/index.js', function() {
    it('should create the LendingLibrary.Index', function() {
        // arrange
        // act
        // assert
        expect(LendingLibrary.People.Index).toBeDefined();
    });
    var toRemove = [];
    var mkContext = function () {
        var ctx = $('<div></div>');
        toRemove.push(ctx);
        $('body').append(ctx);
        return ctx;
    };
    afterEach(function () {
        toRemove.forEach(function (item) {
            item.remove();
        });
    });
    describe('deletePerson', function() {
        it('should be a function on the namespace', function() {
            // arrange
            // act
            // assert
            expect(LendingLibrary.People.Index.deletePerson).toBeDefined();
        });
        it('should call $.post with given parameters and url', function() {
            // arrange
            var deleteUrl = '/foo/bar';
            var id = 1;
            var deferred = $.Deferred();
            spyOn($, 'post').and.returnValue(deferred.promise());
            // act
            LendingLibrary.People.Index.deletePerson(id, deleteUrl);
            // assert
            expect($.post).toHaveBeenCalledWith(deleteUrl, { id: id });
        });
        it('should call sweet alerts given a successful call to $.post', function() {
            // arrange
            var deleteUrl = '/foo/bar';
            var id = 1;
            var deferred = $.Deferred();
            spyOn($, 'post').and.returnValue(deferred.promise());
            spyOn(window, 'sweetAlert');
            // act
            LendingLibrary.People.Index.deletePerson(id, deleteUrl);
            deferred.resolve();
            // assert
            expect(window.sweetAlert).toHaveBeenCalled();
        });
    });
});