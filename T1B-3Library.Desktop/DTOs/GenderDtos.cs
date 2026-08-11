namespace T1B_3Library.Desktop.DTOs
{
    /// <summary>
    /// DTO para representar uma Categoria retornada pela API.
    /// </summary>
    public class GenderResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        /// <summary>Quantidade de games nesta categoria (calculado pela API)</summary>
        public int BookCount { get; set; }
    }

    /// <summary>
    /// DTO para criação de uma nova Categoria.
    /// Enviado no corpo do POST /api/categories.
    /// </summary>
    public class CreateGenderDto
    {
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para atualização de uma Categoria existente.
    /// Enviado no corpo do PUT /api/categories/{id}.
    /// </summary>
    public class UpdateGenderDto
    {
        public string Name { get; set; } = string.Empty;
    }
}
