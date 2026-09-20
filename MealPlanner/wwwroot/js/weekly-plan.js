let selectedDays = [];

function openAddToPlanModal(recipeId, recipeName, recipeImage) {
    const modalRecipeId = document.getElementById('modalRecipeId');
    const modalRecipeName = document.getElementById('modalRecipeName');
    const modalRecipeImage = document.getElementById('modalRecipeImage');
    
    if (modalRecipeId) modalRecipeId.value = recipeId;
    if (modalRecipeName) modalRecipeName.textContent = recipeName;
    if (modalRecipeImage) {
        modalRecipeImage.src = recipeImage;
        modalRecipeImage.onerror = function() {
            this.onerror = null;
            this.src = '/images/no-image.svg';
        };
    }
    
    const modal = document.getElementById('addToPlanModal');
    if (modal) {
        modal.classList.remove('hidden');
        modal.classList.add('flex');
    }
    
    selectedDays = [];
    document.querySelectorAll('.day-button').forEach(btn => {
        btn.className = 'day-button rounded-lg border-2 border-slate-200 bg-white px-2 py-3 text-sm font-medium text-slate-700 transition hover:border-emerald-400 hover:text-emerald-600';
    });
    updateSelectedDaysInput();
}

function closeAddToPlanModal() {
    const modal = document.getElementById('addToPlanModal');
    if (modal) {
        modal.classList.add('hidden');
        modal.classList.remove('flex');
    }
}

function toggleDay(button, dayValue) {
    const index = selectedDays.indexOf(dayValue);
    
    const defaultClass = 'day-button rounded-lg border-2 border-slate-200 bg-white px-2 py-3 text-sm font-medium text-slate-700 transition hover:border-emerald-400 hover:text-emerald-600';
    const selectedClass = 'day-button rounded-lg border-2 border-emerald-500 bg-emerald-50 px-2 py-3 text-sm font-medium text-emerald-600 transition';

    if (index > -1) {
        selectedDays.splice(index, 1);
        button.className = defaultClass;
    } 
    else {
        selectedDays.push(dayValue);
        button.className = selectedClass;
    }
    
    updateSelectedDaysInput();
}

function updateSelectedDaysInput() {
    const input = document.getElementById('selectedDaysInput');
    if (input) {
        input.value = selectedDays.join(',');
    }
}

document.addEventListener('DOMContentLoaded', function() {
    const modal = document.getElementById('addToPlanModal');
    
    if (modal) {
        modal.addEventListener('click', function(e) {
            if (e.target === this) {
                closeAddToPlanModal();
            }
        });
    }
    
    document.addEventListener('keydown', function(e) {
        if (e.key === 'Escape') {
            const openModal = document.getElementById('addToPlanModal');
            if (openModal && !openModal.classList.contains('hidden')) {
                closeAddToPlanModal();
            }
        }
    });
});