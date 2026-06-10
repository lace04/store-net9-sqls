document.addEventListener('DOMContentLoaded', function () {
  var toggle = document.getElementById('menu-toggle');
  var menu = document.getElementById('mobile-menu');
  if (toggle && menu) {
    toggle.addEventListener('click', function () {
      var expanded = toggle.getAttribute('aria-expanded') === 'true';
      toggle.setAttribute('aria-expanded', !expanded);
      menu.classList.toggle('hidden');
    });
  }
});

var deleteTarget = null;

function showConfirmModal(formId, itemName) {
  deleteTarget = document.getElementById(formId);
  document.getElementById('confirm-item-name').textContent = itemName;
  var modal = document.getElementById('confirm-modal');
  var card = document.getElementById('modal-card');

  modal.classList.remove('hidden');
  void modal.offsetWidth;
  modal.classList.add('modal-show');
  card.classList.add('modal-card-show');
}

function hideConfirmModal() {
  var modal = document.getElementById('confirm-modal');
  var card = document.getElementById('modal-card');

  modal.classList.remove('modal-show');
  card.classList.remove('modal-card-show');
  modal.classList.add('modal-hide');
  card.classList.add('modal-card-hide');

  setTimeout(function () {
    modal.classList.add('hidden');
    modal.classList.remove('modal-hide');
    card.classList.remove('modal-card-hide');
    deleteTarget = null;
  }, 200);
}

function submitDelete() {
  if (deleteTarget) deleteTarget.submit();
}
