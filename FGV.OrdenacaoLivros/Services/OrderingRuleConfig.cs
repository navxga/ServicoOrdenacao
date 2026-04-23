using FGV.OrdenacaoLivros.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FGV.OrdenacaoLivros.Services;

public static class OrderingRuleConfig
{
    /// <summary>
    /// Carrega as regras de ordenação a partir de um arquivo JSON.
    /// Permite configurar atributos e direções sem alteração de código.
    /// </summary>
    /// <param name="configPath">Caminho para o arquivo de configuração.</param>
    public static IEnumerable<OrderingRule> Load(string configPath = "appsettings.json")
    {
        if (!File.Exists(configPath))
            throw new Exception($"Arquivo de configuração não encontrado: {configPath}");

        var json = File.ReadAllText(configPath);
        var root = JsonSerializer.Deserialize<JsonElement>(json);

        if (!root.TryGetProperty("OrderingRule", out var rulesElement))
            throw new Exception("Chave 'OrderingRule' não encontrada no arquivo de configuração.");

        // Necessário para deserializar enums por nome ("Title") em vez de valor numérico (0)
        var options = new JsonSerializerOptions() { Converters = { new JsonStringEnumConverter() } };

        return JsonSerializer.Deserialize<IEnumerable<OrderingRule>>(
            rulesElement.GetRawText(), options) ?? throw new Exception("Não foi possível deserializar as regras de ordenação."
        );
    }
}
