function toggleCommentForm() {
    let post = event.srcElement.dataset.post;
    let commentForm = document.querySelector(`#comment-form${post}`);
    commentForm.classList.toggle('post-comment-form-active');
}