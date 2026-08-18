'A simple premium calculator class that calculates insurance premiums based on policy type, state, and number of claims.'
Public Class PremiumCalculator

        'Calculates the insurance premium based on the provided policy type, state, and number of claims.
        '
        '<param name="policyType">The type of insurance policy (e.g., "auto", "home", "life").</param>
        '<param name="state">The state where the policy is issued (e.g., "CA", "TX").</param>
        '<param name="claimsCount">The number of claims made on the policy.</param>
        '<returns>The calculated insurance premium as a Decimal value.</returns>
        Public Function CalculatePremium(policyType As String, state As String, claimsCount As Integer) As Decimal

            Dim baseRate As Decimal = GetBaseRate(policyType)
            Dim stateFactor As Decimal = GetStateFactor(state)
            Dim claimsFactor As Decimal = GetClaimsFactor(claimsCount)

            Dim premium As Decimal = baseRate * stateFactor * claimsFactor

            Return Math.Round(premium, 2)

        End Function

        'Gets the base rate for the specified policy type.
        '
        '<param name="policyType">The type of insurance policy.</param>
        '<returns>The base rate as a Decimal value.</returns>
        Private Function GetBaseRate(policyType As String) As Decimal

            Select Case policyType.ToLower()
                Case "auto"
                    Return 1200D
                Case "home"
                    Return 1800D
                Case "life"
                    Return 500D
                Case Else
                    Return 1000D
            End Select

        End Function

        'Gets the state factor for the specified state.
        '
        '<param name="state">The state where the policy is issued.</param>
        '<returns>The state factor as a Decimal value.</returns>
        Private Function GetStateFactor(state As String) As Decimal

            Select Case state.ToUpper()
                Case "CA", "FL", "NY"
                    Return 1.3D
                Case "TX", "GA"
                    Return 1.1D
                Case "OH"
                    Return 0.95D
                Case Else
                    Return 1D
            End Select

        End Function

        'Gets the claims factor based on the number of claims made on the policy.
        '
        '<param name="claimsCount">The number of claims made on the policy.</param>
        '<returns>The claims factor as a Decimal value.</returns>
        Private Function GetClaimsFactor(claimsCount As Integer) As Decimal

            If claimsCount = 0 Then
                Return 0.9D
            ElseIf claimsCount = 1 Then
                Return 1D
            ElseIf claimsCount = 2 Then
                Return 1.25D
            Else
                Return 1.5D
            End If

        End Function

    End Class
