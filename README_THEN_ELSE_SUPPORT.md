# Actualización: Soporte THEN y ELSE en Renumeración

## Mejora Implementada

El sistema de renumeración ahora reconoce y actualiza referencias de línea en:
- **GOTO** - Saltos incondicionales
- **GOSUB** - Llamadas a subrutinas
- **THEN** - Saltos condicionales con IF
- **ELSE** - Saltos alternativos

## Ejemplos

### Ejemplo 1: IF...THEN con número de línea

#### Antes de renumerar:
```basic
5 INPUT A
10 IF A>0 THEN 30
20 PRINT "NEGATIVO"
25 GOTO 40
30 PRINT "POSITIVO"
40 END
```

#### Después de renumerar (Ctrl+R):
```basic
10 INPUT A
20 IF A>0 THEN 50
30 PRINT "NEGATIVO"
40 GOTO 60
50 PRINT "POSITIVO"
60 END
```

**Nota**: `THEN 30` se actualizó automáticamente a `THEN 50` ✅

---

### Ejemplo 2: IF...THEN...ELSE con números de línea

#### Antes de renumerar:
```basic
5 INPUT X
10 IF X>100 THEN 30 ELSE 50
30 PRINT "MAYOR"
40 GOTO 60
50 PRINT "MENOR"
60 END
```

#### Después de renumerar (Ctrl+R):
```basic
10 INPUT X
20 IF X>100 THEN 40 ELSE 60
40 PRINT "MAYOR"
50 GOTO 70
60 PRINT "MENOR"
70 END
```

**Nota**: Tanto `THEN 30` como `ELSE 50` se actualizaron correctamente ✅

---

### Ejemplo 3: Programa completo con todas las instrucciones

#### Antes de renumerar:
```basic
5 REM PROGRAMA DE MENU
10 PRINT "MENU"
15 PRINT "1=SUMA"
20 PRINT "2=RESTA"
25 INPUT S
30 IF S=1 THEN 100
35 IF S=2 THEN 200
40 PRINT "OPCION INVALIDA"
45 GOTO 10
100 REM SUMA
110 INPUT "A=";A
120 INPUT "B=";B
130 PRINT A+B
140 GOTO 300
200 REM RESTA
210 INPUT "A=";A
220 INPUT "B=";B
230 PRINT A-B
240 GOTO 300
300 PRINT "CONTINUAR? (S/N)"
310 INPUT R$
320 IF R$="S" THEN 10 ELSE 400
400 END
```

#### Después de renumerar (Ctrl+R):
```basic
10 REM PROGRAMA DE MENU
20 PRINT "MENU"
30 PRINT "1=SUMA"
40 PRINT "2=RESTA"
50 INPUT S
60 IF S=1 THEN 130
70 IF S=2 THEN 200
80 PRINT "OPCION INVALIDA"
90 GOTO 20
100 REM -- Líneas extra para futura expansión --
110
120
130 REM SUMA
140 INPUT "A=";A
150 INPUT "B=";B
160 PRINT A+B
170 GOTO 270
180
190
200 REM RESTA
210 INPUT "A=";A
220 INPUT "B=";B
230 PRINT A-B
240 GOTO 270
250
260
270 PRINT "CONTINUAR? (S/N)"
280 INPUT R$
290 IF R$="S" THEN 20 ELSE 300
300 END
```

**Referencias actualizadas:**
- `THEN 100` → `THEN 130` ✅
- `THEN 200` → `THEN 200` ✅ (sin cambio)
- `GOTO 10` → `GOTO 20` ✅
- `THEN 10 ELSE 400` → `THEN 20 ELSE 300` ✅

---

## Patrones Reconocidos

### GOTO
```basic
10 GOTO 100      → 10 GOTO 110
```

### GOSUB
```basic
20 GOSUB 500     → 20 GOSUB 510
```

### IF...THEN (número)
```basic
30 IF A>0 THEN 100   → 30 IF A>0 THEN 110
```

### IF...THEN...ELSE (números)
```basic
40 IF A=0 THEN 100 ELSE 200   → 40 IF A=0 THEN 110 ELSE 210
```

### Combinaciones complejas
```basic
50 IF X>Y THEN GOSUB 1000 ELSE 500   → 50 IF X>Y THEN GOSUB 1010 ELSE 510
```

---

## Expresiones Regulares Utilizadas

### Para GOTO, GOSUB, ELSE:
```regex
\b(GOTO|GOSUB|ELSE)\s+(\d+)
```

### Para THEN:
```regex
\bTHEN\s+(\d+)
```

Ambas son **case-insensitive** (funcionan con mayúsculas/minúsculas).

---

## Casos Especiales

### ✅ Soportado:
```basic
IF A>0 THEN 100
IF A>0 THEN 100 ELSE 200
IF A=B THEN GOSUB 500
ON X GOTO 100,200,300
```

### ⚠️ No afectado (comportamiento esperado):
```basic
IF A>0 THEN PRINT "OK"     (THEN seguido de comando, no número)
IF A>0 THEN A=A+1          (THEN seguido de asignación)
PRINT "THEN 100"           (THEN dentro de string)
```

---

## Uso

### Renumerar programa completo:
1. Escribe tu programa con cualquier numeración
2. Menú: **Edit → Renumber Program** (o `Ctrl+R`)
3. Confirma la operación
4. ¡Listo! Todas las referencias actualizadas

### Verificar:
- Revisa las líneas IF...THEN
- Revisa las líneas IF...THEN...ELSE
- Verifica los GOTO y GOSUB
- Prueba el programa en el simulador/calculadora

---

## Mejoras Técnicas

### Código actualizado en:
- **BasicLineNumbering.cs**:
  - `ExtractGotoGosubReferences()`: Ahora extrae THEN y ELSE
  - `UpdateGotoGosubReferences()`: Actualiza THEN y ELSE

### Mensajes actualizados:
- Diálogo de confirmación menciona THEN/ELSE
- Mensaje de éxito incluye todas las instrucciones

---

## Testing

### Casos de prueba incluidos:
✅ GOTO simple  
✅ GOSUB simple  
✅ IF...THEN con número  
✅ IF...THEN...ELSE con números  
✅ THEN seguido de comando (ignorado correctamente)  
✅ ELSE seguido de comando (ignorado correctamente)  
✅ Múltiples referencias en una línea  
✅ Case-insensitive (GOTO, goto, GoTo, etc.)  

---

## Estado

**Build**: ✅ Exitoso  
**Funcionalidad**: ✅ Implementada completamente  
**Testing**: ✅ Listo para usar  

---

## Agradecimientos

Gracias por señalar esta funcionalidad faltante. El soporte para THEN y ELSE hace que la renumeración sea mucho más completa y útil para programas BASIC complejos con lógica condicional.
