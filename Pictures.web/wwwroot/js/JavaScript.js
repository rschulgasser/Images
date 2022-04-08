$(() => {
    $("#like-button").on('click', function () {

        const id = $("#image-id").val();
        const liked = true;

        $.post('/home/viewimage', { id, liked }, function () {
            console.log(id);
            $("#like-button").attr("disabled", "disabled");
            UpdateLikes(id);

        });


    });
    setInterval(() => {
        const id = $("#image-id").val();
        
        UpdateLikes(id);
    }, 500)

    function UpdateLikes(id) {
      
        $.get('/home/getlikes', { id }, function (numOfLikes) {
          
            $("#likes-count").text(`${numOfLikes}`);


        });
    }
});