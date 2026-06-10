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
  var card = modal.lastElementChild;

  modal.classList.remove('hidden');
  void modal.offsetWidth;
  modal.classList.remove('opacity-0');
  card.classList.remove('scale-95');
}

function hideConfirmModal() {
  var modal = document.getElementById('confirm-modal');
  var card = modal.lastElementChild;

  modal.classList.add('opacity-0');
  card.classList.add('scale-95');

  setTimeout(function () {
    modal.classList.add('hidden');
    deleteTarget = null;
  }, 200);
}

function submitDelete() {
  if (deleteTarget) deleteTarget.submit();
}
