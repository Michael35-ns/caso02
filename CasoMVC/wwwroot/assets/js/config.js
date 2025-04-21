function confirmInscripcion(entityName, entityId, incribirmeUrl) {
    Swal.fire({
        title: `¿Estás seguro de que quieres inscribirte al evento ${entityName}?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: `¡Sí, inscribirme!`
    }).then((result) => {
        if (result.isConfirmed) {
            const finalUrl = incribirmeUrl.replace("{id}", entityId);

            const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

            fetch(finalUrl, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            })
                .then(async response => {
                    const contentType = response.headers.get("content-type");
                    if (contentType && contentType.includes("application/json")) {
                        return response.json();
                    } else {
                        throw new Error("La respuesta no es JSON válida.");
                    }
                })
                .then(data => {
                    if (data.success) {
                        Swal.fire(
                            `${entityName} inscrito`,
                            `Te has inscrito exitosamente a ${entityName}.`,
                            'success'
                        ).then(() => {
                            window.location.href = "../Eventos/Eventos";
                        });
                    } else {
                        Swal.fire(
                            'Error',
                            data.message || `No se pudo inscribir a ${entityName}.`,
                            'error'
                        );
                    }
                })
                .catch(error => {
                    console.error('Detalles del error:', error);
                    Swal.fire(
                        'Error',
                        `Ocurrió un error al inscribirse al evento ${entityName}. Detalles: ${error.message || error}`,
                        'error'
                    );
                });
        }
    });
}
