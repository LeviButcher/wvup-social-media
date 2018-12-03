let buttons = document.querySelectorAll('[data-post]');
buttons.forEach(button => button.addEventListener('click', toggleCommentForm));

function toggleCommentForm() {
    console.dir(this);
    let post = this.dataset.post;
    let commentForm = document.querySelector(`#comment-form${post}`);
    commentForm.classList.toggle('post-comment-form-active');
}